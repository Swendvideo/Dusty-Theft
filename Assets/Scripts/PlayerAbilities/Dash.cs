using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Dash")]
public class Dash : PlayerAbility
{
    [SerializeField] float dashPower;
    public override IEnumerator Activate(Player player)
    {
        Physics2D.IgnoreLayerCollision(7,8, true);
        player.SetSpeedModifier(player.SpeedModifier*dashPower);
        yield return new WaitForSeconds(0.1f);
        player.SetSpeedModifier(player.SpeedModifier/dashPower);
        Physics2D.IgnoreLayerCollision(7,8, false);
        IsReady = false;
        yield return Cooldown();
        IsReady = true;
    }

    public override void RangeVisualLogic(Vector2 mousePosition, Vector2 playerPosition, RectangleGraphic rangeVisual)
    {
        
    }
}
