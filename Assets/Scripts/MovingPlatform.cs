using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 endPoint;
    public float speed;
    public static bool onPlatform = false;
    
    private Rigidbody playerRb;
    private Rigidbody rb;
    private int point;
    private Vector3 startPoint;
    private Vector3 targetPoint;
    private bool isBack;
    private float playerSpeed;
    void Start ()
    {
        playerSpeed = speed + (speed * 0.1f);
        Debug.Log("Player Speed: " + playerSpeed);
        startPoint = transform.position;
        targetPoint = endPoint;
        point = 0;
        isBack = false;
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate ()
    {
        Moving();
        if (playerRb != null && onPlatform)
        {
            if (PlayerController.isMoving)
            {
                return;
            }
            
            playerRb.velocity = transform.forward * playerSpeed;
        }
    }

    private void Moving()
    {
        if (Vector3.Distance(targetPoint, transform.position) < 0.1f)
        {
            if (isBack)
            {
                isBack = false;
                targetPoint = endPoint;
            }
            else
            {
                targetPoint = startPoint;
                isBack = true;
            }
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
        transform.rotation = targetRotation;

        rb.velocity = transform.forward * speed;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onPlatform = true;
            playerRb = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onPlatform = false;
            playerRb = null;
        }
    }
}
