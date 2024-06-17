using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float detectionRadius;
    [SerializeField] float secondsBeforeActivatingAbility;
    [SerializeField] NavMeshAgent navMeshAgent;
    Vector2 patrolPoint;
    bool isAggro;
    Transform target;
    IEnumerator Activate()
    {
        while (true)
        {
            if(isAggro)
            {
                MoveAggro();
            }
            else
            {
                MovePatrol();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.CompareTag("Player"))
        {
            isAggro = true;
            target = other.transform;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.CompareTag("Border"))
        {
            patrolDirection = new Vector2(Random.Range(-1,1), Random.Range(-1,1));
        }
    }

    void GetPatrolPoint()
    {
        
    }
    virtual public void MovePatrol()
    {
        
    }
    virtual public void MoveAggro()
    {
        
    }
    virtual public void UseAbility()
    {

    }
}
