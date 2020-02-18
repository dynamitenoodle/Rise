using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBlast : Ability
{
    public GameObject attackPrefab;
    List<GameObject> attacks = new List<GameObject>();
    List<GameObject> prevAttacks = new List<GameObject>();
    List<Vector2> attackPositions = new List<Vector2>();

    public float speed = Constants.ABILITY_RANGED_ATTACK_DEFAULT;

    public override void Action()
    {
        attackPrefab = Resources.Load<GameObject>("Abilities/magicBlast");
        attacks.Add(Instantiate(attackPrefab, player.transform.position, player.transform.rotation));
        BulletScript bulletScript = attacks[attacks.Count-1].GetComponent<BulletScript>();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = Vector3.Normalize(mousePosition - (Vector2)player.transform.position);

        bulletScript.SetAttributes(direction, speed);

        attackPositions.Add(player.transform.position);
    }

    IEnumerator WatchAttacks()
    {
        for(;;)
        {
            if  (attacks.Count <= 0)
        }
    }
}
