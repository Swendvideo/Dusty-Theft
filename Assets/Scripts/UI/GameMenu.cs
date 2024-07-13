using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] Slider difficultySlider;
    [SerializeField] TMP_Text difficultyText;
    [SerializeField] Shop shop;
    public void OnEnterDungeonButton()
    {
        GameManager.Instance.DataManager.ChangeDifficulty((int)difficultySlider.value+5);
        GameManager.Instance.StartGame();
    }

    public void OnDifficultySliderChange()
    {
        difficultyText.text = difficultySlider.value.ToString();
    }

    public void OnShopButton()
    {
        shop.UpdateShop();
    }
}
