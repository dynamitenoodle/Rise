﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier_ElectricRune : Modifier
{
    EnvironmentManager environmentManager;
    private void Start()
    {
        environmentManager = GameObject.Find(Constants.GAMEOBJECT_NAME_ENVIRONMENTMANAGER).GetComponent<EnvironmentManager>();
    }
    public override void Action(ModifierInfo modifierInfo)
    {
        if (environmentManager == null)
        {
            environmentManager = GameObject.Find(Constants.GAMEOBJECT_NAME_ENVIRONMENTMANAGER).GetComponent<EnvironmentManager>();
        }
        for (int i = 0; i < modifierInfo.points.Length; i++)
        {
            environmentManager.AddElectric(modifierInfo.points[i], modifierInfo.radius * 2f);
        }
    }

    public override ModifierStartAction StartAction(Ability ability)
    {
        return ModifierStartAction.remain_after;
    }

    public override bool ModifyPlayer(PlayerScript player)
    {
        return false;
    }
}
