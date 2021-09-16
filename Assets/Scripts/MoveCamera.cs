using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform CameraPlaceholder;

    private void Update()
    {
        transform.position = CameraPlaceholder.transform.position;
    }
}
