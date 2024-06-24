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
    [SerializeField] float speed;
    [SerializeField] float detectionRadius;
    [SerializeField] float secondsBeforeActivatingAbility;
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] int ExcludeRangeAbs; 
    Vector3 patrolPoint;
    bool isAggro;
    Transform target;
    public void Init()
    {
        navMeshAgent.updateRotation = false;
		navMeshAgent.updateUpAxis = false;
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
                yield return new WaitUntil(() => navMeshAgent.remainingDistance < 0.3f);
            }
            while(isAggro)
            {
                MoveAggro();
                yield return null;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.CompareTag("Player"))
        {
            navMeshAgent.autoBraking = false;
            isAggro = true;
            target = other.transform;
            MoveAggro();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.CompareTag("Player"))
        {
            navMeshAgent.autoBraking = true;
            isAggro = false;
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
    virtual public void UseAbility()
    {

    }
}
