using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    //Gun Stuff
    public GameObject player;
    public GameObject bulletSpawnPoint;
    public GameObject bullet;
    public float shootRate = 2;
    public float reloadTime = 3;

    public TextMeshProUGUI ammoText;
    
    public Transform orientation;
    public LayerMask groundMask;
    public float speed;
    public float jumpForce;
    public float groundDrag;
    public float airDrag;
    public float movementMultiplier;
    public float airMultiplier;

    public static bool isMoving;
    
    private Rigidbody rb;
    private Vector3 moveDirection;
    private float movementX;
    private float movementY;
    private float distance = 1.05f;
    private float groundDistance = 0.4f;
    private bool isGrounded;
    private float elapsedTime;
    private float ammo = 3;

    private Vector2 movementVector;
    private void Awake()
    {
        player = gameObject;
        rb = GetComponent<Rigidbody>();
        rb.drag = groundDrag;
        rb.freezeRotation = true;
    }

    private void Start()
    {
        isMoving = false;
        UpdateUI();
        elapsedTime = 0.0f;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, groundMask);
        //isGrounded = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), distance);
        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * (distance) , Color.red);
        //Debug.Log(isGrounded);

        if (movementVector.x != 0f || movementVector.y != 0f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
        ControlDrag();
        Reload();
    }
    
    void OnMove(InputValue movementValue)
    {
        movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump()
    {
        MovingPlatform.onPlatform = false;
        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnFire()
    {
        ShootBullet();
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
    
    private void ShootBullet()
    {
        //Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        if (elapsedTime >= shootRate && ammo > 0)
        {
            ammo -= 1;
            Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
            elapsedTime = 0.0f;
            
        }
        
        UpdateUI();
    }

    private void Reload()
    {
        if (ammo == 0)
        {
            ammoText.text = "reloading";
        }
        
        if (elapsedTime >= reloadTime && ammo == 0)
        {
            ammo = 3;
            UpdateUI();
        }
    }
    
    
    private void UpdateUI()
    {
        ammoText.text = ammo.ToString() + "/3";
    }
}
