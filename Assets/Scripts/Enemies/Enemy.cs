using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class EnemyType
{
    public string name;
    public Enemy EnemyPrefab;
    public int DifficultyCost;
}

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] float chaiseSpeed;
    [SerializeField] float patrolSpeed;
    [SerializeField] float detectionRadius;
    [SerializeField] protected float abilityCooldown;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] CircleCollider2D detectionCollider;
    [SerializeField] int ExcludeRangeAbs; 
    protected Transform playerTransform;
    bool isAggro;
    Transform target;
    public void Init()
    {
        navMeshAgent.updateRotation = false;
		navMeshAgent.updateUpAxis = false;
        detectionCollider.radius = detectionRadius;
    }

    public void Activate()
    {
        StartCoroutine(Process());
    }

    IEnumerator Process()
    {
        isAggro = false;
        while (true)
        {

            while(!isAggro)
            {
                MovePatrol();
                yield return new WaitUntil(() => navMeshAgent.remainingDistance < 0.3f || isAggro);
            }
            while(isAggro)
            {
                MoveAggro();
                yield return null;
            }
        }
    }

    virtual public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.CompareTag("Player"))
        {
            playerTransform = other.transform;
            navMeshAgent.autoBraking = false;
            isAggro = true;
            target = other.transform;
            navMeshAgent.speed = chaiseSpeed;
            StartCoroutine(AbilityCycle());
        }
    }

    virtual public void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.CompareTag("Player"))
        {
            navMeshAgent.autoBraking = true;
            isAggro = false;
            navMeshAgent.speed = patrolSpeed;
        }
        
    }

    Vector3 GetPatrolPoint()
    {
        NavMeshHit hit;
        var randomNum = Enumerable.Range(-5, 11).Where(x => (x <= -ExcludeRangeAbs || ExcludeRangeAbs >= 3)).ToArray();
        NavMesh.SamplePosition(new Vector3(transform.position.x + randomNum[UnityEngine.Random.Range(0,randomNum.Length)] ,transform.position.y + randomNum[UnityEngine.Random.Range(0,randomNum.Length)]) , out hit, 5f, NavMesh.AllAreas);
        return hit.position;
    }
    virtual public void MovePatrol()
    {
        Vector3 point = GetPatrolPoint();
        navMeshAgent.SetDestination(point);
    }
    virtual public void MoveAggro()
    {
        navMeshAgent.SetDestination(target.position);
    }

    virtual public void ActivateAbility()
    {
        navMeshAgent.SetDestination(target.position);
    }

    virtual public IEnumerator AbilityCycle()
    {
        while(true)
        {
            yield return new WaitForSeconds(abilityCooldown);
            ActivateAbility();
        }
    }
}
