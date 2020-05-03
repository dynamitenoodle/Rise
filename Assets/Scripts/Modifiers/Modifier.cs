using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier
{
    public enum ModifierStartAction
    {
        delete_after,
        remain_after
    }

    public abstract bool ModifyPlayer(PlayerScript playerScript);
    public abstract ModifierStartAction StartAction(Ability ability);
    public abstract void Action(ModifierInfo modifierInfo);
}
