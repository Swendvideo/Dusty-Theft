using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<EnemyType> enemyTypes;
    public GameManager Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;

    }
}
