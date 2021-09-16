using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public float distance;
    public float delay;
    public Rigidbody rb;
    
    private Rigidbody platformRb;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isOnPlatform;
    
    private void Awake()
    {
        platformRb = gameObject.GetComponent<Rigidbody>();
        startPos = transform.position;
        endPos = new Vector3(startPos.x + distance, transform.position.y, transform.position.z);
    }
    
    void MovePlatform()
    {
        if (Vector3.Distance(transform.position, endPos) > 0.1)
        {
            platformRb.velocity = new Vector3(speed, 0, 0);
        }
        else
        {
            //platformRb.velocity = new Vector3(0, 0, 0);
        }
        
    }    
    
    private void FixedUpdate()
    {
        MovePlatform();
        if (isOnPlatform)
        {
            rb.velocity = platformRb.velocity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered");
        Debug.Log(rb.gameObject.name);

        isOnPlatform = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
        isOnPlatform = false;
    }
}
