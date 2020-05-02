using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier_CoolDown : Modifier
{
    public override void Action(ModifierInfo modifierInfo)
    {
        return;
    }

    public override ModifierStartAction StartAction(Ability ability)
    {
        ability.coolDown /= 2;

        return ModifierStartAction.delete_after;
    }

    public override void ModifyPlayer(PlayerScript player)
    {
        return;
    }
}
