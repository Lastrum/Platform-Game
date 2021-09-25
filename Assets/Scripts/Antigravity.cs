using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Antigravity : MonoBehaviour
{
    public float force = 5;
    private Rigidbody rb;
    
    private void OnTriggerStay(Collider other)
    {
        rb = other.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, force, rb.velocity.z);
    }
}
