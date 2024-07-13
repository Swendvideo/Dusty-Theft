using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Archer : Enemy
{
    bool isAbilityCycleActive = false;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float projectileSpeed;
    Coroutine abilityCycle;
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
            navMeshAgent.isStopped = false;
            SetDestination(playerTransform.position);
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

    protected override void OnTriggerExit2D(Collider2D other)
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

    protected override IEnumerator AbilityCycle()
    {
        while(true)
        {
            yield return new WaitForSeconds(abilityCooldown);
            ActivateAbility();
        }
    }

    protected override void ActivateAbility()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerTransform.position - transform.position, int.MaxValue, 1 << 6);
        Debug.DrawRay(hit.point, hit.point - (Vector2)transform.position, Color.green, 10);
        var arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity, GameManager.Instance.LocationManager.activeArea.transform);
        arrow.transform.right = playerTransform.position - arrow.transform.position;
        arrow.transform.DOMove(hit.point, (hit.point - (Vector2)transform.position).magnitude/projectileSpeed).SetEase(Ease.Linear).OnComplete(() => arrow.gameObject.tag = "Untagged");
        
    }


}
