using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] Animator loadScreen;
    [SerializeField] List<EnemyType> enemyTypes;
    [SerializeField] 

    private void SpawnEnemy(Bounds bounds, Enemy enemy)
    {
        float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float randomY =  UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        Vector3 pos = new Vector3(randomX, randomY,0);
        var e = Instantiate(enemy, pos,new Quaternion());
        e.Init();
    }

    void AreaCleared()
    {
        sceneCompleted = true;
    }

    void NextArea(Location location)
    {
        if (isSceneActive)
        {
            Destroy(activeLocation);
        }
        var loc = Instantiate(location.Area);
        loc.Exit += AreaCleared;
    }

    public IEnumerator GameProcess(int difficulty)
    {
        int locationCost = difficulty;
        while(locationCost > 0)
        {
            loadScreen.SetBool("IsLoading", true);
            var AvailableLocations = Locations.Where(l => locationCost - l.DifficultyCost >= 0);
            Location randomLocation = AvailableLocations.ElementAt( UnityEngine.Random.Range(0,AvailableLocations.Count()));
            NextArea(randomLocation);
            yield return new WaitUntil(() => sceneCompleted);
            
            

            
        }
    }
}
