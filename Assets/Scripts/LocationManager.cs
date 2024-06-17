using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Location
{
    public GameObject Area;
    public int difficulty;
}
public class LocationManager : MonoBehaviour
{

    public List<Location> Locations;
    [SerializeField] Animator loadScreen;

    public IEnumerator GameProcess(int difficulty)
    {
        int locationCost = difficulty;
        while(locationCost > 0)
        {
            loadScreen.SetBool("IsLoading", true);
            var AvailableLocations = Locations.Where(x => locationCost - x.difficulty >= 0);
            Location randomLocation = AvailableLocations.ElementAt(Random.Range(0,AvailableLocations.Count()));
            



        }
        return null;
    }
}
