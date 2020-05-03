using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier_HyperSpeedUp : Modifier
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
        player.speed *= 4f;
        player.maxSpeed *= 4f;
        return true;
    }
}
