using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public Transform orientation;
    public LayerMask groundMask;
    public float speed;
    public float jumpForce;
    public float groundDrag;
    public float airDrag;
    public float movementMultiplier;
    public float airMultiplier;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float movementX;
    private float movementY;
    private float distance = 1.05f;
    private float groundDistance = 0.4f;
    private bool isGrounded;
    private void Awake()
    {
        player = gameObject;
        rb = GetComponent<Rigidbody>();
        rb.drag = groundDrag;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, groundMask);
        //isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), distance);
        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * (distance) , Color.red);
        //Debug.Log(isGrounded);
        
        ControlDrag();
    }
    
    void OnMove(InputValue movementValue)
    {
        Debug.Log("Moving");
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump(InputValue movementValue)
    {
        Debug.Log("Jumped");
        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
    
    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }
    
    void Movement()
    {
        moveDirection = orientation.forward * movementY + orientation.right * movementX;

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * speed * movementMultiplier, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * speed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
        
    }

    private void FixedUpdate()
    {
        Movement();
    }
}
