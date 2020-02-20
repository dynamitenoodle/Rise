using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_MagicBlast : Ability
{
    public GameObject attackPrefab;

    public float speed = Constants.ABILITY_RANGED_ATTACK_DEFAULT;
    public float lifeSpan = 2.0f;

    public override void Setup()
    {
        coolDown = 2.0f;
    }

    public override void Action()
    {
        attackPrefab = Resources.Load<GameObject>("Abilities/magicBlast");
        GameObject attack = Instantiate(attackPrefab, player.transform.position, player.transform.rotation);
        BulletScript bulletScript = attack.GetComponent<BulletScript>();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = Vector3.Normalize(mousePosition - (Vector2)player.transform.position);

        bulletScript.SetAttributes(direction, speed);
        bulletScript.SetTimeout(lifeSpan, true, true);
        bulletScript.SetCallback(FinishAttack);
    }

    public void FinishAttack(GameObject attackObj)
    {
        ModifierInfo modifierInfo = new ModifierInfo(player);

        Vector2[] points = new Vector2[1];
        points[0] = attackObj.transform.position;

        modifierInfo.radius = 2f;
        modifierInfo.points = points;

        foreach (Modifier modifier in modifiers)
        {
            modifier.Action(modifierInfo);
        }
    }
}
