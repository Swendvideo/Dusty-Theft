using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float maxHealth;
    [SerializeField] float immunityDuration;
    [SerializeField] Animator animator;
    bool isImmune = false;
    public float Health
    {
        get;
        private set;
    }

    public void Init()
    {
        Health = maxHealth;
    }

    void Update()
    {
        float y = Input.GetAxis("Vertical") * speed;
        float x = Input.GetAxis("Horizontal")* speed;
        rb.velocity = new Vector3(x, y,0)*Time.deltaTime*speed;
    }

    void TakeDamage(float damage)
    {
        if (!isImmune)
        {
            Health -= damage;
            Debug.Log(Health);
            GameManager.Instance.PlayerUI.UpdateHealthIndicator(Health);
            if (Health <= 0)
            {
                GameManager.Instance.Death();
            }
            StartCoroutine(Immunity());
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.CompareTag("Enemy"))
        {
            TakeDamage(1f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.CompareTag("Arrow"))
        {
            TakeDamage(0.5f);
        }
    }

    IEnumerator Immunity()
    {
        isImmune = true;
        animator.SetBool("IsImmune", true);        
        yield return new WaitForSeconds(immunityDuration);
        animator.SetBool("IsImmune", false);
        isImmune = false;
    }
}
