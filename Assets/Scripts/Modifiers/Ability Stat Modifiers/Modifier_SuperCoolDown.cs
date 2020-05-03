using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier_SuperCoolDown : Modifier
{
    public override void Action(ModifierInfo modifierInfo)
    {
        return;
    }

    public override ModifierStartAction StartAction(Ability ability)
    {
        ability.coolDown /= 4;

        return ModifierStartAction.delete_after;
    }

    public override bool ModifyPlayer(PlayerScript player)
    {
        return false;
    }
}
