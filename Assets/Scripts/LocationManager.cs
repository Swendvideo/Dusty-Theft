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
}
public class LocationManager : MonoBehaviour
{
    private GameObject activeLocation;
    private bool isSceneActive;
    private bool sceneCompleted;
    [SerializeField] List<Location> Locations;
    [SerializeField] NavMeshSurface navMeshSurface;
    [SerializeField] Animator loadScreen;
    List<Enemy> enemies = new List<Enemy>();

    public void AreaCleared()
    {
        sceneCompleted = true;
        DestroyEnemies();
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

    IEnumerator SpawnEnemies(Area area)
    {
        int difficultyCoin = GameManager.Instance.Difficulty;
        while(difficultyCoin > 0)
        {
            var availableEnemies = GameManager.Instance.enemyTypes.Where(e => difficultyCoin >= e.DifficultyCost).ToArray();
            Enemy enemy = availableEnemies[UnityEngine.Random.Range(0, availableEnemies.Count())].EnemyPrefab;
            var e = Instantiate(enemy,GetRandomPointInBound(area.enemySpawnArea.bounds), new Quaternion());
            e.Init();
            enemies.Add(e);
            difficultyCoin -= 1;
            yield return new WaitUntil(() => e.isActiveAndEnabled);
        }
    }
    
    void ActivateEnemies()
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.Activate();
        }
    }
    void DestroyEnemies()
    {
        foreach(Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();
    }
    public IEnumerator GameProcess(int difficulty)
    {
        loadScreen.gameObject.SetActive(true);
        int locationCost = difficulty;
        while(locationCost > 0)
        {
            ActivateLoadingScreen();
            yield return new WaitForSeconds(0.20f);
            GameManager.Instance.menu.gameObject.SetActive(false);
            var AvailableLocations = Locations.Where(l => locationCost - l.DifficultyCost >= 0);
            Location randomLocation = AvailableLocations.ElementAt(UnityEngine.Random.Range(0,AvailableLocations.Count()));
            if (isSceneActive)
            {
                Destroy(activeLocation);
            }
            var loc = Instantiate(randomLocation.Area);
            navMeshSurface.BuildNavMeshAsync();
            yield return null;
            yield return SpawnEnemies(loc);
            isSceneActive = true;
            DisableLoadingScreen();
            ActivateEnemies();
            yield return new WaitUntil(() => sceneCompleted);   
            
            

            
        }
    }
}
