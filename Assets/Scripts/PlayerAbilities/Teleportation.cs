using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[CreateAssetMenu]
public class Teleportation : PlayerAbility 
{
    [SerializeField] Color rangeVisualGreen;
    [SerializeField] Color rangeVisualRed;
    bool requirementsFulfilled = false;
    public override void Activate(PointerEventData pointerEventData, Player player)
    {

    }

    public override void RangeVisualLogic(Vector2 mousePosition, Vector2 playerPosition, RectangleGraphic rangeVisual)
    {
        Debug.Log(mousePosition);
        var hit = Physics2D.Raycast(mousePosition,Vector2.zero);
        if (hit.transform != null)
        {
            Debug.Log(hit.transform.name);
            Debug.Log($"{hit.point.x} - {hit.point.y}");
            if(hit.transform.CompareTag("GameFloor") && ((mousePosition - playerPosition).magnitude < Range))
            {
                rangeVisual.color = rangeVisualGreen;
                requirementsFulfilled = true;
            }
            else
            {
                rangeVisual.color = rangeVisualRed;
                requirementsFulfilled = false;
            }
        }
    }
}
