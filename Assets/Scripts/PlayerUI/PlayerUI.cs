using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] List<Transform> fullHearts;
    [SerializeField] Transform halfHeart;
    [SerializeField] PlayerAbilityUI playerAbilityUI;
    [SerializeField] PlayerHealthUI playerHealthUI;
    [SerializeField] PlayerEscapeUI playerEscapeIndicatorUI;

    public void UpdateHealthIndicator(float health)
    { 
        playerHealthUI.UpdateHealthIndicator(health);
    }

    public void UpdateAbilityIndicator(float fillAmount)
    {
        playerAbilityUI.UpdateAbilityIndicator(fillAmount);
    }

    public void UpdateEscapeIndicator(float fillAmount)
    {
        playerEscapeIndicatorUI.UpdateEscapeIndicator(fillAmount);
    }

    public void HideAbilityIndicator(bool hide)
    {
        playerAbilityUI.gameObject.SetActive(!hide);   
    }
    public void SetAbilityIcon(Sprite icon)
    {
        playerAbilityUI.SetAbilityIcon(icon);
    }
}
