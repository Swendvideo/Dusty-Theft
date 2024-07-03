using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Archer : Enemy
{
    bool isClearView;
    bool isAbilityCycleActive = false;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float projectileSpeed;
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
        }
        else
        {
            if(!isAbilityCycleActive)
            {
                abilityCycle = StartCoroutine(AbilityCycle());
                isAbilityCycleActive = true;
            }
            navMeshAgent.isStopped = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            navMeshAgent.isStopped = false;
            base.OnTriggerExit2D(other);
            if(isAbilityCycleActive)
            {
                StopCoroutine(abilityCycle);
                isAbilityCycleActive = false;
                abilityCycle = null;
            }
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerTransform.position - transform.position, int.MaxValue, 1 << 6);
        Debug.DrawRay(hit.point, hit.point - (Vector2)transform.position, Color.green, 10);
        var arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity, GameManager.Instance.LocationManager.activeArea.transform);
        arrow.transform.right = playerTransform.position - arrow.transform.position;
        arrow.transform.DOMove(hit.point, (hit.point - (Vector2)transform.position).magnitude/projectileSpeed).SetEase(Ease.Linear).OnComplete(() => arrow.gameObject.tag = "Untagged"); 
        Debug.Log((hit.point, (hit.point - (Vector2)transform.position).magnitude/projectileSpeed));
        
    }


}
