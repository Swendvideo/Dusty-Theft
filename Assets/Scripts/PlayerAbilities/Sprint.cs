using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Abilities/Sprint")]
public class Sprint : PlayerAbility
{
    [SerializeField] float sprintPower;
    [SerializeField] float sprintDuration;
    public override IEnumerator Activate(Player player)
    {
        player.SetSpeedModifier(player.SpeedModifier*sprintPower);
        yield return new WaitForSeconds(sprintDuration);
        player.SetSpeedModifier(player.SpeedModifier/sprintPower);
        IsReady = false;
        yield return Cooldown();
        IsReady = true;
    }

    public override void RangeVisualLogic(Vector2 mousePosition, Vector2 playerPosition, RectangleGraphic rangeVisual)
    {
        
    }
}
