using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Assasin : Enemy
{
    bool isAbilityCycleActive = false;
    [SerializeField] float leapPower;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Image leapIndicator;
    [SerializeField] Gradient indicatorGradient;
    [SerializeField] Color leapIndicatorColorBeginning;
    [SerializeField] Color leapIndicatorColorFinal;
    Sequence colorChanging;
    Coroutine abilityCycle;

    public override void Init()
    {
        colorChanging = DOTween.Sequence();
        base.Init();
        colorChanging.Append(leapIndicator.DOColor(leapIndicatorColorFinal, abilityCooldown)).SetAutoKill(false).Pause();
    }

    protected override void MoveAggro()
    {
        if(Physics2D.Raycast(transform.position, playerTransform.position - transform.position, (playerTransform.position - transform.position).magnitude, 1 << 6))
        {
            if(isAbilityCycleActive)
            {
                StopCoroutine(abilityCycle);
                isAbilityCycleActive = false;
                abilityCycle = null;
            }
            DeactivateIndicator();
        }
        else
        {
            if(!isAbilityCycleActive)
            {
                abilityCycle = StartCoroutine(AbilityCycle());
                isAbilityCycleActive = true;
            }
            leapIndicator.transform.right = new Vector3(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y, 0);
        }
        base.MoveAggro();
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(isAbilityCycleActive)
            {
                StopCoroutine(abilityCycle);
                isAbilityCycleActive = false;
                abilityCycle = null;
                rb.velocity = Vector2.zero;
                rb.totalForce = Vector2.zero;
            }
            navMeshAgent.isStopped = false;
            base.OnTriggerExit2D(other);
            DeactivateIndicator();

        }

    }

    void ActivateIndicator()
    {
        leapIndicator.color = leapIndicatorColorBeginning;
        leapIndicator.gameObject.SetActive(true);
        colorChanging.Rewind(false);
        colorChanging.Play();
    }

    void DeactivateIndicator()
    {
        leapIndicator.gameObject.SetActive(false);
        colorChanging.Rewind(false);
    }

    protected override IEnumerator AbilityCycle()
    {
        while(true)
        {
            ActivateIndicator();
            yield return new WaitForSeconds(abilityCooldown);
            leapIndicator.color = leapIndicatorColorBeginning;
            rb.WakeUp();
            ActivateAbility();
            yield return new WaitForSeconds(0.5f);
            rb.Sleep();
        }
    }

    protected override void ActivateAbility()
    {
        rb.AddForce((playerTransform.position-transform.position).normalized * leapPower, ForceMode2D.Impulse);
    }


}
