using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public string itemName;
    public float cost;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.LocationManager.AddTreasure(this);
            Destroy(gameObject);
        }
    }
}
