using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class PlayerAbility : ScriptableObject
{
    public string abilityName;
    public Sprite sprite;
    public float abilityCooldown;
    public bool IsMouseBased;
    public float Range;
    public bool IsReady;
    public bool requirementsFulfilled = false;
    public IEnumerator Cooldown()
    {
        float timer = 0;
        while(timer<=abilityCooldown)
        {
            GameManager.Instance.PlayerUI.UpdateAbilityIndicator(timer/abilityCooldown);
            yield return null;
            timer += Time.deltaTime;
        }
        GameManager.Instance.PlayerUI.UpdateAbilityIndicator(1);
    }

    abstract public IEnumerator Activate(Player player);

    abstract public void RangeVisualLogic(Vector2 mousePosition, Vector2 playerPosition, RectangleGraphic rangeVisual);
}
