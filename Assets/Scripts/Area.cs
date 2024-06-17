using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> enemySpawnArea;
    public void Init()
    {   
        
    }

    IEnumerator Setup()
    {
        yield return null;
    }

    Vector2 GetRandomPointInBound(Bounds bounds)
    {
        
        var x = Random.Range(bounds.min.x,bounds.max.x);
        var y = Random.Range(bounds.min.y,bounds.max.y); 
        return new Vector2(x,y);
    }
}
