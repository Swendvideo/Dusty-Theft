using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[CreateAssetMenu]
public class Teleportation : PlayerAbility 
{
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
        var hits = Physics2D.RaycastAll(mousePosition,Vector2.zero);
        if (hits != null)
        {
            if(hits.Any(h => h.transform.CompareTag("GameFloor")) && !hits.Any(h => h.transform.CompareTag("Wall")) && ((mousePosition - playerPosition).magnitude < Range))
            {
                SetRequirementsFulfilled(true, rangeVisual);
            }
            else
            {
                SetRequirementsFulfilled(false, rangeVisual);
            }
        }
    }
}
