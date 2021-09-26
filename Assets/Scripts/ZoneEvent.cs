using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZoneEvent : MonoBehaviour
{
    public static UnityEvent Zone;
    public static UnityEvent CancelCall;

    public static bool zoneFired;
    
    private int counter = 0;
    void Start()
    {
        zoneFired = false;
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

    private void Ping()
    {
        counter++;
        if (counter > 3) counter = 3;
    }

    private void Update()
    {
        Debug.Log("Check if NPC Ready: " + counter);
        if (counter == 3)
        {
            counter = 0;
            NPCController.ZoneReady.Invoke();
        }
    }
    
}
