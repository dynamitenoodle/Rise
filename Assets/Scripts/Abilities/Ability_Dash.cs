using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Dash : Ability
{
    Vector2 direction;
    float dashSpeed;
    bool dashing = false;
    List<Vector2> dashPoints;
    int maxDashCount = 5;

    public override void Setup()
    {
        lastUseTime = Time.time;
        coolDown = 2f;
    }

    public override void Action()
    {
        if (Time.time - lastUseTime < coolDown || dashing) { return; }

        dashPoints = new List<Vector2>();

        direction = GetKeyDirection();
        if (direction == Vector2.zero)
            direction = GetMouseDirection();

        direction *= dashSpeed;
        Debug.Log($"Dashing with direction {direction}");
        dashing = true;
        playerScript.invulnerable = true; //probably use something else here
        StartCoroutine(DoDash());
    }

    IEnumerator DoDash()
    {
        for (;;)
        {
            Debug.Log("test");
            dashPoints.Add(player.transform.position);
            player.transform.position += (Vector3)direction * Time.deltaTime;

            if (dashPoints.Count >= maxDashCount)
            {
                ModifierInfo modifierInfo = GetModifierInfo();
                foreach (Modifier modifier in modifiers)
                {
                    modifier.Action(modifierInfo);
                }

                playerScript.invulnerable = false;
                dashing = false;
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private ModifierInfo GetModifierInfo()
    {
        ModifierInfo modifierInfo = new ModifierInfo(player);
        modifierInfo.points = dashPoints.ToArray();
        modifierInfo.radius = 1;

        return modifierInfo;
    }

    private Vector2 GetMouseDirection()
    {
        Vector2 dir = Vector2.zero;
        dir = Input.mousePosition;
        return dir.normalized;
    }

    private Vector2 GetKeyDirection()
    {
        Vector2 dir = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            dir.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir.x += 1;
        }

        return dir;
    }

    public override void SetCooldown()
    {
        lastUseTime = Time.time;
        abilityUIManager.SetAbilityCooldown(abilitySlot, coolDown);
    }

}
