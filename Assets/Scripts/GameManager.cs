using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<EnemyType> EnemyTypes;
    public LocationManager LocationManager;
    public DataManager DataManager;
    public PlayerUI PlayerUI;
    public CameraFollow PlayerCamera;
    public static GameManager Instance
    {
        get;
        private set;
    }
    public Transform menu;

    public void StartGame()
    {
        StartCoroutine(LocationManager.GameProcess());
    }

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public void Death()
    {
        
    }
}
