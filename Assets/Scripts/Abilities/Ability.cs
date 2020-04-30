using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{

    protected GameObject player;
    protected PlayerScript playerScript;
    protected List<Modifier> modifiers;
    public float coolDown;
    protected float lastUseTime;
    protected AbilityUIManager abilityUIManager;
    public int abilitySlot;

    private void Start()
    {
        player = GameObject.FindWithTag(Constants.TAG_PLAYER);
        playerScript = player.GetComponent<PlayerScript>();
        abilityUIManager = GameObject.Find(Constants.GAMEOBJECT_NAME_CANVAS).GetComponent<AbilityUIManager>();
        modifiers = new List<Modifier>();

        Setup();
    }

    public abstract void Action();
    public abstract void SetCooldown();
    public abstract void Setup();
    public void AddModifier(Modifier modifier)
    {
        //Debug.Log("adding modifier!");
        //check for modifier combinations
        Modifier.ModifierStartAction action = modifier.StartAction(this);

        if (action == Modifier.ModifierStartAction.delete_after)
        {
            //dont add
        }
        else if (action == Modifier.ModifierStartAction.remain_after)
        {
            //add to list
            modifiers.Add(modifier);
        }
    }
}
