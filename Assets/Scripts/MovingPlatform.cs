using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    
    private Transform startPos;
    private float step;
    private Vector3 target;
    private bool readyCheck;

    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        readyCheck = false;
        startPos = transform;
        target = new Vector3(startPos.position.x, startPos.position.y + 10, startPos.position.z);
    }
    private void Update()
    {
        if (readyCheck)
        {
            step =  speed * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, target, step );
            //rb.MovePosition(Vector3.up * Time.deltaTime * speed);
            rb.velocity = new Vector3(0, speed, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        readyCheck = true;
    }
    
    private void OnTriggerExit(Collider other)
    {
        readyCheck = false;
    }
}
