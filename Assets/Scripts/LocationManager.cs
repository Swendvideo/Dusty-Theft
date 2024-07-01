using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NavMeshPlus.Components;
using UnityEngine;

[Serializable]
public class Location
{
    public Area Area;
    public int DifficultyCost;
    public int maxEnemies;
}
public class LocationManager : MonoBehaviour
{
    private GameObject activeLocation;
    private bool isSceneActive;
    private bool sceneCompleted;
    [SerializeField] List<Location> Locations;
    [SerializeField] NavMeshSurface navMeshSurface;
    [SerializeField] Animator loadScreen;
    [SerializeField] Player playerPrefab;
    List<Enemy> enemies = new List<Enemy>();
    public Area activeArea;
    
    public void AreaCleared()
    {
        sceneCompleted = true;
        Debug.Log("TEST");
    }
    void ActivateLoadingScreen()
    {
        loadScreen.SetBool("IsLoading", true);
    }

    void DisableLoadingScreen()
    {
        loadScreen.SetBool("IsLoading", false);
    }

    Vector3 GetRandomPointInBound(Bounds bounds)
    {
        var x = UnityEngine.Random.Range(bounds.min.x,bounds.max.x);
        var y = UnityEngine.Random.Range(bounds.min.y,bounds.max.y); 
        return new Vector3(x,y,0);
    }

    IEnumerator SpawnEnemies(Area area, int maxEnemies)
    {
        int difficultyCoin = GameManager.Instance.DataManager.GetEnemyCoinDependingOnDifficulty();
        while((difficultyCoin > 0) || (enemies.Count < maxEnemies))
        {
            var availableEnemies = GameManager.Instance.EnemyTypes.Where(e => difficultyCoin >= e.DifficultyCost).ToArray();
            EnemyType randomEnemyType = availableEnemies[UnityEngine.Random.Range(0, availableEnemies.Count())];
            var e = Instantiate(randomEnemyType.EnemyPrefab,GetRandomPointInBound(area.enemySpawnArea.bounds), new Quaternion());
            e.Init();
            enemies.Add(e);
            difficultyCoin -= randomEnemyType.DifficultyCost;
            Debug.Log("spawned");
            yield return new WaitUntil(() => e.isActiveAndEnabled);
        }
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("TTT");
            DestroyEnemiesAndArea();
        }
    }

    void ActivateEnemies()
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.Activate();
        }
    }
    void DestroyEnemiesAndArea()
    {
        Debug.Log("TTT");
        foreach(Enemy enemy in enemies)
        {
            enemy.StopAllCoroutines();
            Destroy(enemy.gameObject);
        }
        Destroy(activeArea.gameObject);
        enemies.Clear();
    }
    public IEnumerator GameProcess()
    {
        loadScreen.gameObject.SetActive(true);
        int locationCost = GameManager.Instance.DataManager.Difficulty;
        Player player = Instantiate(playerPrefab);
        player.Init();
        player.gameObject.SetActive(false);
        while(locationCost > 0)
        {
            ActivateLoadingScreen();
            yield return new WaitForSeconds(0.3f);
            GameManager.Instance.PlayerUI.UpdateHealthIndicator(player.Health);
            if(activeArea != null)
            {
                DestroyEnemiesAndArea();
            }
            yield return null;
            GameManager.Instance.menu.gameObject.SetActive(false);
            var AvailableLocations = Locations.Where(l => locationCost - l.DifficultyCost >= 0);
            Location randomLocation = AvailableLocations.ElementAt(UnityEngine.Random.Range(0,AvailableLocations.Count()));
            locationCost -= randomLocation.DifficultyCost;
            if (isSceneActive)
            {
                Destroy(activeLocation);
            }
            var loc = Instantiate(randomLocation.Area);
            activeArea = loc;
            navMeshSurface.BuildNavMeshAsync();
            player.transform.position = loc.spawnPoint.position;
            GameManager.Instance.PlayerCamera.SetTarget(player.transform);
            yield return null;
            yield return SpawnEnemies(loc, randomLocation.maxEnemies);
            isSceneActive = true;
            player.gameObject.SetActive(true);
            DisableLoadingScreen();
            ActivateEnemies();
            sceneCompleted = false;
            yield return new WaitUntil(() => sceneCompleted);
        }
    }
}
