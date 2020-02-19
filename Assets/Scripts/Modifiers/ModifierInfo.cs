using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierInfo
{
    public GameObject player = null;
    public PlayerScript playerScript = null;
    public Vector2[] points = null;
    public float radius = -1;

    public ModifierInfo (GameObject player)
    {
        this.player = player;
        this.playerScript = player.GetComponent<PlayerScript>();
    }

}
