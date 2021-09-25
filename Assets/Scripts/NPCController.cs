using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    public GameObject player;
    public GameObject healthBarUI;
    public Slider slider;
    
    public float health = 3;
    public float healthBarRotationSpeed;

    public float knockback = 5;
    
    private Rigidbody rb;
    private Quaternion healthBarRotation;
    private float maxHealth;
    private void Start()
    {
        maxHealth = health;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateHealth();
    }

    void OnCollisionEnter(Collision collision)
    {
        //Reduce health
        if (collision.gameObject.tag == "Bullet")
        {
            rb.AddForce(collision.transform.forward * knockback, ForceMode.Impulse);
            health -= 1;
            CheckHealth();
        }
    }

    private void CheckHealth()
    {
        if (health <= 0)
        {
            Explode();
        }
    }

    private void UpdateHealth()
    {
        //healthBarUI.gameObject.transform.rotation = player.transform.position;
        healthBarRotation = Quaternion.LookRotation(healthBarUI.transform.position - player.transform.position);
        healthBarUI.transform.rotation = Quaternion.Slerp(healthBarUI.transform.rotation, healthBarRotation, Time.deltaTime * healthBarRotationSpeed);
        slider.value = health / maxHealth;
    }
    
    protected void Explode()
    {
        rb.freezeRotation = false;
        rb.AddForce(Vector3.right * 2);
        
        Destroy(gameObject, 1.5f);
    }
}
