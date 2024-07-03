using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityUI : MonoBehaviour
{
    PlayerAbility playerAbility => GameManager.Instance.DataManager.selectedAbility;
    [SerializeField] Image cooldownIndicator;
    [SerializeField] RectangleGraphic abilityIcon;
    [SerializeField] Color abilityIsReady;
    [SerializeField] Color abilityNotReady;

    public void SetAbilityIcon(Sprite icon)
    {
        abilityIcon.Texture = icon.texture;
    }

    public void UpdateAbilityIndicator(float fillAmount)
    {
        cooldownIndicator.fillAmount = fillAmount;
        if(fillAmount == 1)
        {
            cooldownIndicator.color = abilityIsReady;
        }
        else
        {
            cooldownIndicator.color = abilityNotReady;
        }
    }



}
