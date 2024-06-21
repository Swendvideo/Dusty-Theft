using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExit : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.locationManager.AreaCleared();
            Debug.Log("test2");
        }
    }
}
