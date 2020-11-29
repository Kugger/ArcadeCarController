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
    public float turnStrength = 180f;
    public float gravityForce = 10f;
    public float dragOnGround = 3f;

    private float speedInput;
    private float turnInput;

    private bool grounded;

    public LayerMask GroundMask;


    // Update is called once per frame
    void Update()
    {
        transform.position = sphereCollider.transform.position - new Vector3(0, 0.4f, 0);

        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAcceleration * 1000f;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * backwardAcceleration * 1000f;
        }

        turnInput = Input.GetAxis("Horizontal");
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));

    }

    private void FixedUpdate()
    {
        
    }
}
