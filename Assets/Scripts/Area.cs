using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> enemySpawnArea;
    [SerializeField] Transform spawnPoint;
    [SerializeField] AreaExit areaExit;
    public event Action Exit;   
    public void Init(int difficulty)
    {   
        areaExit.exit += ExitLocation;
        SpawnEnemies(difficulty);
    }

    void SpawnEnemies(int difficulty)
    {
        while(difficulty > 0)
        {

        }
    }
    
    void ExitLocation()
    {
        Exit.Invoke();
    }
    
    IEnumerator Setup()
    {
        yield return null;
    }

    Vector2 GetRandomPointInBound(Bounds bounds)
    {
        var x = UnityEngine.Random.Range(bounds.min.x,bounds.max.x);
        var y = UnityEngine.Random.Range(bounds.min.y,bounds.max.y); 
        return new Vector2(x,y);
    }
}
