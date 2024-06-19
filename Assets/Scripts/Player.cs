using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
