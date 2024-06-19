using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExit : MonoBehaviour
{
    public event Action exit;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            exit.Invoke();
        }
    }
}
