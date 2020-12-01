﻿using System.Collections;
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
        // Kart model (box for now 30/11/2020) copies the position of the sphere
        transform.position = sphereCollider.transform.position - new Vector3(0, 0.4f, 0);

        // 
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAcceleration * 35f;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * backwardAcceleration * 35f;
        }

        turnInput = Input.GetAxis("Horizontal");
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));
    }

    private void FixedUpdate()
    {
        // Forward Acceleration
        sphereCollider.AddForce(kartModel.transform.forward * speedInput, ForceMode.Acceleration);

        // Gravity
        sphereCollider.AddForce(Vector3.down * gravityForce * 10, ForceMode.Acceleration);

        // Steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y, 0), Time.deltaTime * 5f);

        // Rotating the model based on ground
        Ray ray = new Ray(transform.position - (transform.up * 1f), -transform.up);
        RaycastHit hitInfo;

        Debug.DrawRay(ray.origin, ray.origin + ray.direction * 100f, Color.green);

        if (Physics.Raycast(ray, out hitInfo, 2.0f, GroundMask))
        {
            kartModel.rotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation;
        }
    }
}
