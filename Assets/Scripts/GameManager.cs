using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<EnemyType> enemyTypes;
    public LocationManager locationManager;
    public static GameManager Instance
    {
        get;
        private set;
    }
    public int Difficulty;
    public Transform menu;

    public void StartGame()
    {
        StartCoroutine(locationManager.GameProcess(Difficulty));
    }

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;

    }
}
