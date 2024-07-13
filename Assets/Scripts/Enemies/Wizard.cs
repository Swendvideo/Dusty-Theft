using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Wizard : Enemy
{
    bool isAbilityCycleActive = false;
    [SerializeField] GameObject iciclePrefab;
    [SerializeField] float minProjectileSpawnDistance;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileBackDistance;
    [SerializeField] float projectileBackSpeed;
    [SerializeField] float slownessWhenInSightMultiplier;
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
                navMeshAgent.speed = chaiseSpeed;
            }
            SetDestination(playerTransform.position);
        }
        else
        {
            if(!isAbilityCycleActive)
            {
                abilityCycle = StartCoroutine(AbilityCycle());
                isAbilityCycleActive = true;
                navMeshAgent.speed = chaiseSpeed/slownessWhenInSightMultiplier;
            }
            
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
        var t = Mathf.PI*2/20;
        float currentAngle = 0;
        List<Vector2> points = new List<Vector2>();
        Vector2 icicleSpawnPoint = new Vector2();
        RaycastHit2D raycastHit;
        for(int i = 0; i < 20; i++)
        {
            Vector3 direction = new Vector3(Mathf.Cos(currentAngle),Mathf.Sin(currentAngle));
            raycastHit = Physics2D.Raycast(playerTransform.position, direction, minProjectileSpawnDistance, 1<<6);
            if(raycastHit.transform == null)
            {
                Vector2 point = playerTransform.position + direction * minProjectileSpawnDistance; 
                points.Add(point);
            }
            else
            {
                points.Add(raycastHit.point);
            }
            currentAngle += t;
        }
        var suitablePoints = points.Where(p => ((Vector2)playerTransform.position - p).magnitude > minProjectileSpawnDistance).ToList();
        if(suitablePoints.Count() > 0)
        {
            icicleSpawnPoint = suitablePoints[Random.Range(0,suitablePoints.Count())];
            Debug.Log(icicleSpawnPoint);
            icicleSpawnPoint = icicleSpawnPoint - ((Vector2)playerTransform.position - icicleSpawnPoint)*0.1f;
            Debug.Log(icicleSpawnPoint);
            Vector3 playerPos = playerTransform.position;
            GameObject icicle = Instantiate(iciclePrefab,icicleSpawnPoint, Quaternion.identity ,GameManager.Instance.LocationManager.activeArea.transform);
            raycastHit = Physics2D.Raycast(icicle.transform.position, playerPos-icicle.transform.position, int.MaxValue, 1<<6);
            icicle.transform.right = playerPos - icicle.transform.position;
            icicle.transform.DOMove(icicle.transform.position + ((Vector3)icicleSpawnPoint - playerPos).normalized * projectileBackDistance,((Vector2)icicle.transform.position - (Vector2)(icicle.transform.position + ((Vector3)icicleSpawnPoint - playerPos).normalized * projectileBackDistance)).magnitude/projectileBackSpeed)
            .OnComplete(() => icicle.transform.DOMove(raycastHit.point, ((Vector2)icicle.transform.position - raycastHit.point).magnitude/projectileSpeed).SetEase(Ease.Linear)
            .OnComplete(() => Destroy(icicle)));
        }
    }




}
