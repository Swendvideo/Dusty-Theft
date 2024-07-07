using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerUI PlayerUI;
    public ResultPanel ResultPanel;
    public Transform Menu;
        [SerializeField] TMP_Text MoneyIndicator;

    public void EndGame(string headline, double time, int itemsCollected, int revenue)
    {
        ResultPanel.SetEndResult(headline,time,itemsCollected,revenue);
        Menu.gameObject.SetActive(true);
        UpdateMoneyIndicator();
    }

    public void UpdateMoneyIndicator()
    {
        MoneyIndicator.text = GameManager.Instance.DataManager.Money.ToString();
    }
}
