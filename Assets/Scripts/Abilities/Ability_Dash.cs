using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Dash : Ability
{
    Vector2 direction;
    float dashDistance = 2f;
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
        if (direction.Equals(Vector2.zero))
        {
            direction = GetMouseDirection();
        }

        direction *= dashDistance;
        dashing = true;
        playerScript.invulnerable = true; //probably use something else here
        StartCoroutine(DoDash());
        SetCooldown();
    }

    IEnumerator DoDash()
    {
        bool finished = false;
        do
        {
            dashPoints.Add(player.transform.position);
            player.transform.position += (Vector3)direction / maxDashCount;

            if (dashPoints.Count >= maxDashCount)
            {
                ModifierInfo modifierInfo = GetModifierInfo();
                foreach (Modifier modifier in modifiers)
                {
                    modifier.Action(modifierInfo);
                }

                playerScript.invulnerable = false;
                dashing = false;
                finished = true;
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
        } while (!finished);
    }

    private ModifierInfo GetModifierInfo()
    {
        ModifierInfo modifierInfo = new ModifierInfo(player);
        modifierInfo.points = dashPoints.ToArray();
        modifierInfo.radius = 2.5f;

        return modifierInfo;
    }

    private Vector2 GetMouseDirection()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return (mousePos - (Vector2)player.transform.position).normalized;
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
