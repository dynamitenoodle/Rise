using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{

    protected GameObject player;
    protected List<Modifier> modifiers;

    private void Start()
    {
        player = GameObject.FindWithTag(Constants.TAG_PLAYER);
    }

    public abstract void Action();
    public void AddModifier(Modifier modifier)
    {
        //check for modifier combinations

        //add to list
        modifiers.Add(modifier);
    }
}
