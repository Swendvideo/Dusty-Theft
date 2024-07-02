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
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(abilityCooldown);
        IsReady = true;
    }

    virtual public void Activate(Player player)
    {
        
    }

    abstract public void Activate(PointerEventData pointerEventData, Player player);

    abstract public void RangeVisualLogic(Vector2 mousePosition, Vector2 playerPosition, RectangleGraphic rangeVisual);
}
