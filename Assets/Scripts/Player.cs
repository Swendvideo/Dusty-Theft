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
    [SerializeField] float timeToEscape;
    [SerializeField] Animator animator;
    [SerializeField] PlayerAbility playerAbility;
    bool isImmune = false;

    bool isRangeVisualActive;
    private float escapeTimer;
    private bool hasEscaped = false;
    public float Health
    {
        get;
        private set;
    }
    public float SpeedModifier
    {
        get;
        private set;
    }

    public void Init()
    {
        Health = maxHealth;
        playerAbility = GameManager.Instance.DataManager.selectedAbility;
        playerAbility.IsReady = true;
        playerAbility.requirementsFulfilled = false;
    }

    void FixedUpdate()
    {
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(x, y,0)*Time.deltaTime*speed*SpeedModifier;
    }
    
    void Update()
    {
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
                    StartCoroutine(playerAbility.Activate(this));
                }
            }
        }
        if(rangeVisual.gameObject.activeInHierarchy)
        {
            playerAbility.RangeVisualLogic(Camera.main.ScreenToWorldPoint(Input.mousePosition),transform.position,rangeVisual);
        }
        if(Input.GetMouseButtonDown(0) && playerAbility.requirementsFulfilled && playerAbility.IsMouseBased && playerAbility.IsReady)
        {
            StartCoroutine(playerAbility.Activate(this));
            TweakRangeVisual(playerAbility.Range,!rangeVisual.gameObject.activeInHierarchy);
        }
        if(Input.GetKey(KeyCode.R))
        {
            escapeTimer += Time.deltaTime;
            if(escapeTimer >= timeToEscape && hasEscaped == false)
            {
                hasEscaped = true;
                GameManager.Instance.LocationManager.Escape();
            }
            SetSpeedModifier(0.5f);
        }
        else
        {
            SetSpeedModifier(1f);
            escapeTimer = 0;
        }
        GameManager.Instance.PlayerUI.UpdateEscapeIndicator(escapeTimer/timeToEscape);
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

    void SetSpeedModifier(float value)
    {
        SpeedModifier = value;
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
        if(!rangeVisual.gameObject.activeInHierarchy)
        {
            playerAbility.SetRequirementsFulfilled(false, rangeVisual);
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
