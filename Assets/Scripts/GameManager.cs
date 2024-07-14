using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LocationManager LocationManager;
    public DataManager DataManager;
    public UIManager UIManager;
    public CameraFollow PlayerCamera;
    public static GameManager Instance
    {
        get;
        private set;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartGame()
    {
        LocationManager.StartProcess();
    }

    public void ContinueGame()
    {
        
    }

    public void NewGame()
    {

    }

    void Awake()
    {
        Instance = this;
    }
}
