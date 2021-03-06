﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvironmentElement : MonoBehaviour
{
    protected EnvironmentManager environmentManager;
    public bool inUse;
    private void Start()
    {
        environmentManager = GameObject.Find(Constants.GAMEOBJECT_NAME_ENVIRONMENTMANAGER).GetComponent<EnvironmentManager>();
        inUse = false;
    }
    public abstract void EnvironmentAction(EnemyScript enemyScript);
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (inUse) return;

        inUse = true;
        if (environmentManager == null)
        {
            environmentManager = GameObject.Find(Constants.GAMEOBJECT_NAME_ENVIRONMENTMANAGER).GetComponent<EnvironmentManager>();
        }
        else if (collision.gameObject.tag == Constants.TAG_ENEMY)
        {
            EnvironmentAction(collision.gameObject.GetComponent<EnemyScript>());
        }
        else if (environmentManager.CheckForEnvironmentTag(collision.gameObject.tag))
        {
            environmentManager.UpdateEnvironmentElements(this.gameObject, collision.gameObject);
        }
        inUse = false;
    }
}
