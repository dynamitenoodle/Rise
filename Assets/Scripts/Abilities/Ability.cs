using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{

    protected GameObject player;
    protected List<Modifier> modifiers;
    protected float coolDown;

    private void Start()
    {
        player = GameObject.FindWithTag(Constants.TAG_PLAYER);
        modifiers = new List<Modifier>();

        Setup();
    }

    public abstract void Action();
    public abstract void Setup();
    public void AddModifier(Modifier modifier)
    {
        Debug.Log("adding modifier!");
        //check for modifier combinations

        //add to list
        modifiers.Add(modifier);
    }
}
