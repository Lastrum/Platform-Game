using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    public float rotationSpeed = 5f;
    public float lifeTime = 3;
    public GameObject child;
    
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        child.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    
    void OnCollisionEnter(Collision collision)
    {   
        ContactPoint contact = collision.contacts[0];
        //Instantiate(Explosion, contact.point, Quaternion.identity);
        Destroy(gameObject);
    }
}
