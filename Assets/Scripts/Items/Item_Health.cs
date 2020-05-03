using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Health : ItemPickup
{
    public override void Action(PlayerScript player)
    {
        player.HealPlayer(10);
    }
}
