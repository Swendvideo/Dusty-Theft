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
    public override IEnumerator Activate(Player player)
    {
        if(requirementsFulfilled)
        {
            IsReady = false;
            player.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            yield return Cooldown();
            IsReady = true;
        }
    }

    public override void RangeVisualLogic(Vector2 mousePosition, Vector2 playerPosition, RectangleGraphic rangeVisual)
    {
        var hit = Physics2D.Raycast(mousePosition,Vector2.zero);
        if (hit.transform != null)
        {
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
