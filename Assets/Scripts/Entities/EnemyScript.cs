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
    [SerializeField] float attackRange = 1f;
    Vector3 velocity;
    Vector3 direction;
    GameObject player;

    // attack stuff
    bool attacking;
    Vector3 attackDir;
    [SerializeField] float attackTimerMax = 1.5f;
    [SerializeField] float attackDelay = .5f;
    [SerializeField] float attackSpacing = 1.5f;
    float attackTimer;
    [SerializeField] GameObject attackPrefab;
    GameObject attack;
    [SerializeField] float kickBack = 2.5f;

    // ranged attack info
    [SerializeField] bool isMelee = true;
    bool fired = false;
    [SerializeField] float bulletSpeed = .2f;

    // health
    [SerializeField] float healthMax = 2;
    float health;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        velocity = Vector3.zero;
        health = healthMax;
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

        // Attacking stuff
        if (attacking)
        {
            attackTimer += Time.deltaTime;

            // Create the attack when it is time
            if (attackTimer > attackDelay && ((isMelee && attack == null) || !isMelee))
            {
                Attack();
            }

            // Reset the attack timer
            if (attackTimer >= attackTimerMax)
            {
                if (isMelee)
                    Destroy(attack);

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
        else if (attackRange > playerDis)
        {
            attacking = true;
            attackDir = (player.transform.position - transform.position).normalized;
            return true;
        }

        return false;
    }

    void Attack()
    {
        if (isMelee)
        {
            // Melee Attack
            attack = Instantiate(attackPrefab);
            attack.transform.position = transform.position + (attackDir * attackSpacing);
            attack.transform.right = attackDir;

            velocity += attackDir * (speed * kickBack);

            // Checking if player gets hit
            if (player.GetComponent<Renderer>().bounds.Intersects(attack.GetComponent<Renderer>().bounds))
                player.GetComponent<PlayerScript>().GetHit();
        }

        else if (!fired && !isMelee)
        {
            // Range Attack
            attack = Instantiate(attackPrefab);
            attack.transform.position = transform.position + (attackDir * attackSpacing);
            attack.transform.right = attackDir;
            attack.GetComponent<BulletScript>().SetAttributes(attackDir, bulletSpeed);

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