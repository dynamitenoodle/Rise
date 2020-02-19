using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier_FlameRune : Modifier
{
    public override void Action(ModifierInfo modifierInfo)
    {
        EnvironmentManager environmentManager = GameObject.Find(Constants.GAMEOBJECT_NAME_ENVIRONMENTMANAGER).GetComponent<EnvironmentManager>();
        for (int i = 0; i < modifierInfo.points.Length; i++)
        {
            environmentManager.AddFire(modifierInfo.points[i], modifierInfo.radius);
        }
    }
}
