using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dash : PlayerAbility
{
    [SerializeField] float dashPower;
    public override IEnumerator Activate(Player player)
    {
        var rb = player.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(7,8, true);
        player.SetSpeedModifier(player.SpeedModifier*dashPower);
        yield return new WaitForSeconds(0.1f);
        player.SetSpeedModifier(player.SpeedModifier/dashPower);
        Physics2D.IgnoreLayerCollision(7,8, false);
        Debug.Log(new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")));
        IsReady = false;
        yield return Cooldown();
        IsReady = true;
    }

    public override void RangeVisualLogic(Vector2 mousePosition, Vector2 playerPosition, RectangleGraphic rangeVisual)
    {
        
    }
}
