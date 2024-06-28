using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] List<Transform> fullHearts;
    [SerializeField] Transform halfHeart;

    public void UpdateHealthIndicator(float health)
    { 
        foreach(Transform heart in fullHearts)
        {
            heart.gameObject.SetActive(false);
        }
        halfHeart.gameObject.SetActive(false);
        for(int i = 0; i< Mathf.FloorToInt(health); i++)
        {
            fullHearts[i].gameObject.SetActive(true);
        }
        if(health - Mathf.FloorToInt(health) > 0.01)
        {
            halfHeart.gameObject.SetActive(true);
        }
    }
}
