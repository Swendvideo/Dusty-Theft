using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] RectangleGraphic rangeVisual;
    [SerializeField] float maxHealth;
    [SerializeField] float immunityDuration;
    [SerializeField] float timeToEscape;
    [SerializeField] Animator animator;
    [SerializeField] PlayerAbility playerAbility;
    bool isImmune = false;

    bool isRangeVisualActive;
    bool isRKeyPressed = false;
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
        if(GameManager.Instance.DataManager.selectedAbility == null)
        {
            GameManager.Instance.UIManager.PlayerUI.HideAbilityIndicator(true);
        }
        else
        {
            playerAbility = GameManager.Instance.DataManager.selectedAbility;
            GameManager.Instance.UIManager.PlayerUI.HideAbilityIndicator(false);
            playerAbility.IsReady = true;
            playerAbility.requirementsFulfilled = false;
        }
        SpeedModifier = 1;
    }

    void FixedUpdate()
    {
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(x, y,0)*Time.deltaTime*speed*SpeedModifier;
    }
    
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && (playerAbility != null))
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
        if(playerAbility != null)
        {
            if(Input.GetMouseButtonDown(0) && playerAbility.requirementsFulfilled && playerAbility.IsMouseBased && playerAbility.IsReady)
            {
                StartCoroutine(playerAbility.Activate(this));
                TweakRangeVisual(playerAbility.Range,!rangeVisual.gameObject.activeInHierarchy);
            }
        }
        if(Input.GetKey(KeyCode.R))
        {
            escapeTimer += Time.deltaTime;
            if(escapeTimer >= timeToEscape && hasEscaped == false)
            {
                hasEscaped = true;
                GameManager.Instance.LocationManager.Escape();
            }
            if(!isRKeyPressed)
            {
                SetSpeedModifier(SpeedModifier/2);
                isRKeyPressed = true;
            }
        }
        else if(isRKeyPressed)
        {
            SetSpeedModifier(SpeedModifier*2);
            escapeTimer = 0;
            isRKeyPressed = false;
        }
        GameManager.Instance.UIManager.PlayerUI.UpdateEscapeIndicator(escapeTimer/timeToEscape);
    }

    void TakeDamage(float damage)
    {
        if (!isImmune)
        {
            Health -= damage;
            Debug.Log(Health);
            GameManager.Instance.UIManager.PlayerUI.UpdateHealthIndicator(Health);
            if (Health <= 0)
            {
                GameManager.Instance.LocationManager.Death();
            }
            StartCoroutine(Immunity());
        }
    }

    public void SetSpeedModifier(float value)
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
        //Physics2D.IgnoreLayerCollision(7,8, true);        
        yield return new WaitForSeconds(immunityDuration);
        //Physics2D.IgnoreLayerCollision(7,8, false);
        animator.SetBool("IsImmune", false);
        isImmune = false;
    }
}
