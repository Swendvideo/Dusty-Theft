using System;
using System.Collections;
using System.Linq;
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
    [SerializeField] protected float patrolSpeed;
    [SerializeField] protected float patrolDetectionRadius;
    [SerializeField] protected float chaseDetectionRadius;
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

    virtual protected void OnTriggerEnter2D(Collider2D other)
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

    virtual protected void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.CompareTag("Player"))
        {
            SetDestination(playerTransform.position);
            navMeshAgent.autoBraking = true;
            navMeshAgent.speed = patrolSpeed;
            isAggro = false;
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
    virtual protected void MovePatrol()
    {
        Vector3 point = GetPatrolPoint();
        SetDestination(point);
    }
    virtual protected void MoveAggro()
    {
        SetDestination(target.position);
    }



    public void DisableAndDestroy()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    protected void SetDestination(Vector3 targetPos) // NavMeshPlus x issue
    {
		if(Mathf.Abs(transform.position.x - targetPos.x) < 0.0001f)
        {
            Vector3 driftPos;
            NavMeshHit hit;
            if(NavMesh.SamplePosition(targetPos + new Vector3(0.0001f, 0f, 0f),out hit,0, NavMesh.AllAreas))
            {
                driftPos = targetPos + new Vector3(0.0001f, 0f, 0f);
            }
            else if(NavMesh.SamplePosition(targetPos + new Vector3(-0.0001f, 0f, 0f),out hit,0, NavMesh.AllAreas))
            {
                driftPos = targetPos + new Vector3(-0.0001f, 0f, 0f);
            }
            else
            {
                driftPos = GetPatrolPoint();
            }
            navMeshAgent.SetDestination(driftPos);
        }
        else
        {
            navMeshAgent.SetDestination(targetPos);
        }
    }

    virtual protected void ActivateAbility()
    {
        
    }

    virtual protected IEnumerator AbilityCycle()
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
