using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class NPCController : MonoBehaviour
{
    public GameObject player;
    public GameObject healthBarUI;
    public Slider slider;

    public GameObject zone;
    
    public float health = 3;
    public float healthBarRotationSpeed;
    public float lockOnDistance;
    public float knockback = 5;

    public float speed;
    public Rigidbody rb;
    
    private Quaternion healthBarRotation;
    private float maxHealth;
    private bool chaseMode;
    private bool lockOnMode;
    private bool zoneSpawned;
    private bool isWaiting;
    private string whatLevel;
    
    public static UnityEvent ZoneReady;
    
    private void Start()
    {
        isWaiting = true;
        zoneSpawned = false;
        chaseMode = true;
        lockOnMode = false;
        maxHealth = health;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Physics.gravity);
        StartCoroutine("Waiting");
        
        if (ZoneReady == null)
            ZoneReady = new UnityEvent();

        ZoneReady.AddListener(SpawnZone);
        
    }
    
    private void Update()
    {
        UpdateHealth();
        Debug.Log("Zone: " + !GameObject.FindWithTag("Gravity"));
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
        healthBarRotation = Quaternion.LookRotation(healthBarUI.transform.position - player.transform.position);
        healthBarUI.transform.rotation = Quaternion.Slerp(healthBarUI.transform.rotation, healthBarRotation, Time.deltaTime * healthBarRotationSpeed);
        slider.value = health / maxHealth;
    }
    
    private void Explode()
    {
        rb.velocity = Vector3.zero;
        rb.freezeRotation = false;
        rb.AddForce(Vector3.right * (knockback*2), ForceMode.Impulse);
        Destroy(gameObject, 1.5f);
        ZoneEvent.CancelCall.Invoke();
    }

    private void SpawnZone()
    {
        ShootZone();
    }
    public void ShootZone()
    {
        if (!GameObject.FindWithTag("Gravity"))
        {
            zoneSpawned = false;
        }
        if (!zoneSpawned && !GameObject.FindWithTag("Gravity"))
        {
            Instantiate(zone, player.transform.position, player.transform.rotation);
            zoneSpawned = true;
            StartCoroutine("Reset");
        }
    }
    
    IEnumerator Waiting()
    {
        RaycastHit hit;
        while (isWaiting)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                whatLevel = hit.collider.gameObject.tag;
                Debug.Log("npc hit: " + hit.collider.gameObject.tag);
            }

            if (whatLevel == PlayerController.whatLevel)
            {
                isWaiting = false;
                Debug.Log("Player started to Chase!");
                StartCoroutine("Chase");
            }
            yield return new WaitForSeconds(2);
        }
        yield break;
    }

    IEnumerator Chase()
    {
        yield return new WaitForSeconds(1);
        if (whatLevel != PlayerController.whatLevel) StartCoroutine("Waiting");
        lockOnMode = false;
        while (Vector3.Distance(rb.position, player.transform.position) >= lockOnDistance)
        {
            //transform.LookAt(player.transform.position);
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            rb.velocity = transform.forward * speed;
            rb.AddForce(Physics.gravity);
            yield return null;
        }
        
        lockOnMode = true;
        Debug.Log("Acquired Target Lock On!");
        StartCoroutine("LockOn");
        yield break;
    }

    IEnumerator LockOn()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine("Jump");
        StartCoroutine("CheckInRange");
        ZoneEvent.Zone.Invoke();
        
        yield break;
    }
    
    IEnumerator Jump()
    {
        while (lockOnMode)
        {
            rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
            yield return new WaitForSeconds(1);
        }
        
        yield break;
    }
    
    IEnumerator CheckInRange()
    {
        while (lockOnMode)
        {
            if (Vector3.Distance(rb.position, player.transform.position) >= lockOnDistance)
            {
                yield return new WaitForSeconds(3);
                lockOnMode = false;
                ZoneEvent.CancelCall.Invoke();
                StartCoroutine("Chase");
                yield break;
            }
            yield return new WaitForSeconds(1);
        }
        
        yield break;
    }
    
}
