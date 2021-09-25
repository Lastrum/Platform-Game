using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    
     public float sensX;
     public float sensY;

     public Transform gunTransform;
     public Transform eyeTransform;
     public Transform camTransform = null;
     public Transform playerTransform = null;
     public Transform orientation;
     
    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float movementX;
    float movementY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnLook(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        mouseX = movementVector.x;
        mouseY = movementVector.y;
        
        //Debug.Log(movementVector);
    }
    
    private void Update()
    {
        movementY += mouseX * sensX * multiplier;
        movementX -= mouseY * sensY * multiplier;

        movementX = Mathf.Clamp(movementX, -90f, 90f);
        
        camTransform.transform.rotation = Quaternion.Euler(movementX, movementY, 0);
        gunTransform.transform.rotation = Quaternion.Euler(movementX, movementY, 0);
        eyeTransform.transform.rotation = Quaternion.Euler(movementX, movementY, 0);
        playerTransform.rotation = Quaternion.Euler(0, movementY, 0);
        orientation.transform.rotation = Quaternion.Euler(0, movementY, 0);
    }
}