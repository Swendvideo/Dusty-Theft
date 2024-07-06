using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerUI PlayerUI;
    public ResultPanel ResultPanel;
    public Transform Menu;

    public void EndGame(string headline, double time, int itemsCollected, int revenue)
    {
        ResultPanel.SetEndResult(headline,time,itemsCollected,revenue);
        Menu.gameObject.SetActive(true);
    }

}
