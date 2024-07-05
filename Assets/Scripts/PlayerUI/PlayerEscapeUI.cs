using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEscapeUI : MonoBehaviour
{
    [SerializeField] Image escapeIndicator;
    [SerializeField] float timeToEscape;
    private float timer = 0;

    public void UpdateEscapeIndicator(float fillAmount)
    {
        escapeIndicator.fillAmount = fillAmount;
    }
}
