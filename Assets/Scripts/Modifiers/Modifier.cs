using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier : MonoBehaviour
{
    public enum ModifierStartAction
    {
        delete_after,
        remain_after
    }

    public abstract ModifierStartAction StartAction(Ability ability);
    public abstract void Action(ModifierInfo modifierInfo);
}
