using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier_SpeedUp : Modifier
{
    public override void Action(ModifierInfo modifierInfo)
    {
        return;
    }

    public override ModifierStartAction StartAction(Ability ability)
    {
        return ModifierStartAction.delete_after;
    }

    public override bool ModifyPlayer(PlayerScript player)
    {
        player.speed *= 2f;
        player.maxSpeed *= 2f;
        return true;
    }
}
