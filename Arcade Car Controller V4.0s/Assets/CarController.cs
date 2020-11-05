using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Rigidbodies")]
    public Rigidbody sphereCollider;

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


    // Start is called before the first frame update
    void Start()
    {
        sphereCollider.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAcceleration * 1000f;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * backwardAcceleration * 1000f;
        }

        turnInput = Input.GetAxis("Horizontal");

        if (grounded)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
        }

        transform.position = sphereCollider.transform.position;
    }

    private void FixedUpdate()
    {
        grounded = false;
        RaycastHit hitOn;

        Debug.DrawLine(transform.position + (-transform.up * .5f), Vector3.down, Color.green);

        if (Physics.Raycast(transform.position + (-transform.up * .5f), Vector3.down, out hitOn, 1.1f, GroundMask))
        {
            grounded = true;

            transform.rotation = Quaternion.FromToRotation(transform.up, hitOn.normal) * transform.rotation;
        }

        if (grounded)
        {
            sphereCollider.drag = dragOnGround;

            if (Mathf.Abs(speedInput) > 0)
            {
                sphereCollider.AddForce(transform.forward * speedInput);
            }
        }
        else
        {
            sphereCollider.drag = 0.1f;
            sphereCollider.AddForce(Vector3.up * -gravityForce * 100f);
        }
    }
}
