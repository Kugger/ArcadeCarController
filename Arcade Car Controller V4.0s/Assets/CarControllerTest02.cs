using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerTest02 : MonoBehaviour
{
    [Header("Rigidbodies")]
    public Rigidbody sphereCollider;

    [Header("Transforms & Normals")]
    public Transform kartModel;
    public Transform kartNormal;

    [Header("Parametrs")]
    public float forwardAcceleration = 8f;
    public float backwardAcceleration = 4f;
    public float maxSpeed = 50f;
    public float turnStrength = 10f;
    public float gravityForce = 10f;
    public LayerMask GroundMask;

    private float speedInput;
    private float turnInput;
    private bool isGrounded;

    float speed, currentSpeed;
    float rotate, currentRotate;

    void Update()
    {
        // Kart model (box for now 30/11/2020) copies the position of the sphere
        transform.position = sphereCollider.transform.position - new Vector3(0, 0.4f, 0);

        // Collecting input about acceleration and steering
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAcceleration * 35f;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * backwardAcceleration * 35f;
        }

        /*
        if (Input.GetAxis("Horizontal") != 0) 
        {
            int direction = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs(Input.GetAxis("Horizontal"));
            Steer(direction, amount);
        }
        */

        turnInput = Input.GetAxis("Horizontal");
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));

        //currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
        //rotate = 0f;
    }

    private void FixedUpdate()
    {
        // Forward Acceleration
        sphereCollider.AddForce(kartModel.transform.forward * speedInput, ForceMode.Acceleration);


        // Gravity
        sphereCollider.AddForce(Vector3.down * gravityForce * 10f, ForceMode.Acceleration);


        // Steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y, 0), Time.deltaTime * 5f);
        // transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);


        // Rotating the model based on ground
        Ray ray = new Ray(transform.position - (transform.up * 1f), -transform.up);
        RaycastHit hitInfo;

        Debug.DrawRay(ray.origin, ray.origin + ray.direction * 2f, Color.green);

        isGrounded = false;

        if (Physics.Raycast(ray, out hitInfo, 2.0f, GroundMask))
        {
            isGrounded = true;

            kartNormal.up = Vector3.Lerp(kartNormal.up, hitInfo.normal, Time.deltaTime * 8.0f);
            kartNormal.Rotate(0, transform.eulerAngles.y, 0);
        }

        // Appling greater gravity force while in air
        if(!isGrounded)
        {
            sphereCollider.AddForce(Vector3.down * gravityForce * 15f, ForceMode.Acceleration);
        }
    }

    /*
    public void Steer(int direction, float amount) 
    {
        rotate = ((turnStrength * direction) * amount) / 4f;
    }
    */
}