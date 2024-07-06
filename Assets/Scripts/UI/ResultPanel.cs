using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] TMP_Text headline;
    [SerializeField] TMP_Text timeSpent;
    [SerializeField] TMP_Text itemsCollected;
    [SerializeField] TMP_Text revenue;

    public void SetEndResult(string headline ,double timeSpent, int itemsCollected, int revenue)
    {
        gameObject.SetActive(true);
        var timeSpan = TimeSpan.FromSeconds(timeSpent);
        this.timeSpent.text = $"{timeSpan.Minutes}:{timeSpan.Seconds}";
        this.itemsCollected.text = itemsCollected.ToString();
        this.revenue.text = revenue.ToString();
    }

    public void OnReturnButtonPress()
    {
        gameObject.SetActive(false);
    }

}
