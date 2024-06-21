using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public void OnEnterDungeonButton()
    {
        GameManager.Instance.StartGame();
    }
}
