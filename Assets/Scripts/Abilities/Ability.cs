using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{

    protected GameObject player;
    protected PlayerScript playerScript;
    protected List<Modifier> modifiers;
    protected float coolDown;
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

        //add to list
        modifiers.Add(modifier);
    }
}
