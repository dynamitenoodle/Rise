using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvironmentElement : MonoBehaviour
{
    protected EnvironmentManager environmentManager;
    private void Start()
    {
        environmentManager = GameObject.Find(Constants.GAMEOBJECT_NAME_ENVIRONMENTMANAGER).GetComponent<EnvironmentManager>();
    }
    public abstract void EnvironmentAction();
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (environmentManager == null)
        {
            environmentManager = GameObject.Find(Constants.GAMEOBJECT_NAME_ENVIRONMENTMANAGER).GetComponent<EnvironmentManager>();
        }

        if (environmentManager.CheckForEnvironmentTag(collision.gameObject.tag))
        {
            environmentManager.UpdateEnvironmentElements(this.gameObject, collision.gameObject);
        }
    }
}
