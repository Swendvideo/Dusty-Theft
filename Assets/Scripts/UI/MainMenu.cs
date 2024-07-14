using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnContinueButton()
    {
        gameObject.SetActive(false);
    }

    public void OnEraseProgressButton()
    {
        GameManager.Instance.DataManager.ResetAllData();
        gameObject.SetActive(false);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
