using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Assasin : Enemy
{
    bool isAbilityCycleActive = false;
    [SerializeField] float leapPower;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] RectTransform leapIndicator;
    Coroutine abilityCycle;
    public override void MoveAggro()
    {
        if(Physics2D.Raycast(transform.position, playerTransform.position - transform.position, (playerTransform.position - transform.position).magnitude, 1 << 6))
        {
            if(isAbilityCycleActive)
            {
                StopCoroutine(abilityCycle);
                isAbilityCycleActive = false;
                abilityCycle = null;
            }
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(playerTransform.position);
            leapIndicator.gameObject.SetActive(false);
        }
        else
        {
            if(!isAbilityCycleActive)
            {
                abilityCycle = StartCoroutine(AbilityCycle());
                isAbilityCycleActive = true;
            }
            navMeshAgent.SetDestination(playerTransform.position);
            leapIndicator.gameObject.SetActive(true);
            leapIndicator.right = new Vector3(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y, 0);
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            StopCoroutine(abilityCycle);
            isAbilityCycleActive = false;
            navMeshAgent.isStopped = false;
            base.OnTriggerExit2D(other);
            leapIndicator.gameObject.SetActive(false);

        }

    }

    public override IEnumerator AbilityCycle()
    {
        while(true)
        {
            yield return new WaitForSeconds(abilityCooldown);
            ActivateAbility();
        }
    }

    public override void ActivateAbility()
    {
        rb.AddForce((playerTransform.position-transform.position).normalized * leapPower, ForceMode2D.Impulse);
    }


}
