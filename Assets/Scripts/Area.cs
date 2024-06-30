using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NavMeshPlus.Components;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField] public SpriteRenderer enemySpawnArea;
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public AreaExit areaExit;
    public List<Enemy> enemies = new List<Enemy>();
}
