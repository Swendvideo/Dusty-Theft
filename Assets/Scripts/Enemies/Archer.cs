using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Archer : Enemy
{
    public override void MoveAggro()
    {
        if(Physics2D.Raycast(transform.position, playerTransform.position - transform.position, (playerTransform.position - transform.position).magnitude, 1 << 6))
        {
            Debug.Log("test2");
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(playerTransform.position);
        }
        else
        {
            Debug.Log("test1");
            navMeshAgent.isStopped = true;
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        navMeshAgent.isStopped = false;
        base.OnTriggerExit2D(other);
        navMeshAgent.SetDestination(playerTransform.position);

    }

    public override void ActivateAbility()
    {
        
    }


}
