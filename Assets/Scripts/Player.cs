using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rb;
    void Update()
    {
        float y = Input.GetAxis("Vertical") * speed;
        float x = Input.GetAxis("Horizontal")* speed;
        rb.velocity = new Vector3(x, y,0)*Time.deltaTime*speed;
    }
}
