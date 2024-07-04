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
    public string Name;
    public Enemy EnemyPrefab;
    public int DifficultyCost;
    public bool CanSpawn = true;
}

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float chaiseSpeed;
    [SerializeField] float patrolSpeed;
    [SerializeField] float patrolDetectionRadius;
    [SerializeField] float chaseDetectionRadius;
    [SerializeField] protected float abilityCooldown;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] CircleCollider2D detectionCollider;
    [SerializeField] int ExcludeRangeAbs; 
    protected Transform playerTransform;
    bool isAggro;
    bool isAgentStuck;
    Transform target;
    virtual public void Init()
    {
        navMeshAgent.updateRotation = false;
		navMeshAgent.updateUpAxis = false;
        detectionCollider.radius = patrolDetectionRadius;
    }

    public void Activate()
    {
        StartCoroutine(Process());
    }

    IEnumerator Process()
    {
        isAggro = false;
        StartCoroutine(CheckIfAgentIsStuck());
        while (true)
        {
            while(!isAggro)
            {
                yield return new WaitUntil(() => navMeshAgent.remainingDistance < 0.3f || isAggro || isAgentStuck);
                MovePatrol();
                isAgentStuck = false;
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
            detectionCollider.radius = chaseDetectionRadius;
        }
    }

    virtual public void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.CompareTag("Player"))
        {
            navMeshAgent.SetDestination(playerTransform.position);
            navMeshAgent.autoBraking = true;
            navMeshAgent.speed = patrolSpeed;
            isAggro = false;
            StartCoroutine(CheckIfAgentIsStuck());
            detectionCollider.radius = patrolDetectionRadius;
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
        
    }

    virtual public IEnumerator AbilityCycle()
    {
        while(true)
        {
            yield return new WaitForSeconds(abilityCooldown);
            ActivateAbility();
        }
    }

    IEnumerator CheckIfAgentIsStuck()
    {
        float stopTimer = 0;
        while (navMeshAgent.enabled && !navMeshAgent.isStopped && stopTimer < 0.3f)
        {
            yield return new WaitForFixedUpdate();
            if (navMeshAgent.velocity.magnitude < 0.02f)
            {
                stopTimer += Time.fixedDeltaTime;
            }
            else
            {
                stopTimer = 0;
            }
        }
        isAgentStuck = true;
    }
}
