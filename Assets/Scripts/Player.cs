using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] RectangleGraphic rangeVisual;
    [SerializeField] float maxHealth;
    [SerializeField] float immunityDuration;
    [SerializeField] Animator animator;
    [SerializeField] PlayerAbility playerAbility;
    bool isImmune = false;
    bool isRangeVisualActive;
    public float Health
    {
        get;
        private set;
    }

    public void Init()
    {
        Health = maxHealth;
        playerAbility = GameManager.Instance.DataManager.playerAbilities[0];
    }

    void Update()
    {
        float y = Input.GetAxis("Vertical") * speed;
        float x = Input.GetAxis("Horizontal")* speed;
        rb.velocity = new Vector3(x, y,0)*Time.deltaTime*speed;
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(playerAbility.IsReady)
            {
                if(playerAbility.IsMouseBased)
                {
                    TweakRangeVisual(playerAbility.Range,!rangeVisual.gameObject.activeInHierarchy);   
                }
                else
                {
                    playerAbility.Activate(this);
                }
            }
        }
        if(rangeVisual.gameObject.activeInHierarchy)
        {
            playerAbility.RangeVisualLogic(Camera.main.ScreenToWorldPoint(Input.mousePosition),transform.position,rangeVisual);
        }
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

    void TweakRangeVisual(float range, bool setActive)
    {
        rangeVisual.rectTransform.sizeDelta = new Vector2(range*2, range*2);
        rangeVisual.gameObject.SetActive(setActive);
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
