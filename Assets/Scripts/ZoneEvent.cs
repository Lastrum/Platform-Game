using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZoneEvent : MonoBehaviour
{
    public static UnityEvent Zone;
    public static UnityEvent CancelCall;

    private int counter = 0;
    void Start()
    {
        counter = 0;
        
        if (Zone == null) Zone = new UnityEvent();
        if (CancelCall == null) CancelCall = new UnityEvent();
        
        Zone.AddListener(Ping);
        CancelCall.AddListener(CancelEvent);
    }

    private void CancelEvent()
    {
        counter--;
        if (counter < 0) counter = 0;
    }


    private void Update()
    {
        Debug.Log("Check if NPC Ready: " + counter);
        if (counter >= 3)
        {
            counter = 0;
            NPCController.ZoneReady.Invoke();
        }
    }

    private void Ping()
    {
        counter++;
    }
    
}
