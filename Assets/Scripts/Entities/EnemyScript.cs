using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // attributes
    [SerializeField] float maxSpeed = .1f;
    [SerializeField] float speed = .02f;
    [SerializeField] float friction = .9f;
    [SerializeField] float detectionDistance = 10f;
    Vector3 velocity;
    Vector3 direction;
    GameObject player;

    // attack stuff
    List<Attack> attacks;
    bool attacking;
    int attackRoll;
    float attackTimer;
    GameObject attackGO;
    bool fired = false;
    Vector3 attackDir;

    // health
    [SerializeField] float healthMax = 2;
    float health;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        velocity = Vector3.zero;
        health = healthMax;

        attackRoll = -1;
    }

    // Update is called once per frame
    void Update()
    {
        float playerDis = (player.transform.position - transform.position).magnitude;

        // Where is PLAYER
        if (playerDis < detectionDistance && !AttackCheck(playerDis) && !attacking)
            direction = (player.transform.position - transform.position).normalized;

        // Slowdown if nothing
        if (direction == Vector3.zero)
            velocity *= friction;

        velocity += (direction * speed);

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        if (attackRoll == -1)
            attackRoll = Random.Range(0, attacks.Count);

        // Attacking stuff
        if (attacking)
        {
            attackTimer += Time.deltaTime;

            // Create the attackGO when it is time
            if (attackTimer > attackDelay && ((isMelee && attackGO == null) || !isMelee))
            {
                Attack();
            }

            // Reset the attack timer
            if (attackTimer >= attackTimerMax)
            {
                if (isMelee)
                    Destroy(attackGO);

                else
                    fired = false;

                attackTimer = 0;
                attacking = false;
            }
        }

        // Carry out the math
        transform.position += velocity;
        direction = Vector3.zero;

        if (Mathf.Abs(Vector3.Magnitude(player.transform.position - transform.position)) <= GetComponent<Collider2D>().bounds.extents.x * 2)
        {
            if (player.GetComponent<Collider2D>().bounds.Intersects(GetComponent<Collider2D>().bounds))
            {
                player.GetComponent<PlayerScript>().GetHit();
            }
        }
    }

    // The check for attacking
    bool AttackCheck(float playerDis)
    {
        if (attacking)
            return true;

        // IS IT TIME TO ATTACK
        else if (attacks[attackRoll].attackRange > playerDis)
        {
            attacking = true;
            attackDir = (player.transform.position - transform.position).normalized;
            return true;
        }

        return false;
    }

    void Attack()
    {
        if (attacks[attackRoll].isMelee)
        {
            // Melee Attack
            attackGO = Instantiate(attackPrefab);
            attackGO.transform.position = transform.position + (attackDir * attackSpacing);
            attackGO.transform.right = attackDir;

            velocity += attackDir * (speed * kickBack);

            // Checking if player gets hit
            if (player.GetComponent<Renderer>().bounds.Intersects(attackGO.GetComponent<Renderer>().bounds))
                player.GetComponent<PlayerScript>().GetHit();
        }

        else if (!fired && !isMelee)
        {
            // Range Attack
            attackGO = Instantiate(attackPrefab);
            attackGO.transform.position = transform.position + (attackDir * attackSpacing);
            attackGO.transform.right = attackDir;
            attackGO.GetComponent<BulletScript>().SetAttributes(attackDir, bulletSpeed);

            velocity += attackDir * (speed * kickBack);

            fired = true;
        }
    }

    // Called by other scripts to hit the player
    public void GetHit()
    {
        Debug.Log(gameObject.name + " was hit!");
        health--;

        if (health < 0)
            Destroy(gameObject);
    }
}