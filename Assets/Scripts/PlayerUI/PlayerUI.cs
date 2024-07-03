using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] List<Transform> fullHearts;
    [SerializeField] Transform halfHeart;
    [SerializeField] PlayerAbilityUI playerAbilityUI;
    [SerializeField] PlayerHealthUI playerHealthUI;

    public void UpdateHealthIndicator(float health)
    { 
        playerHealthUI.UpdateHealthIndicator(health);
    }

    public void UpdateAbilityIndicator(float fillAmount)
    {
        playerAbilityUI.UpdateAbilityIndicator(fillAmount);
    }
}
