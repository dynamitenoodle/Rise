using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // attributes
    public float maxSpeed = .1f;
    public float speed = .02f;
    public float friction = .9f;
    public float detectionDistance = 10f;
    public float attackRange = 1f;
    Vector3 velocity;
    Vector3 direction;
    GameObject player;

    // attack stuff
    bool attacking;
    Vector3 attackDir;
    public float attackTimerMax = 1.5f;
    public float attackDelay = .5f;
    public float attackSpacing = 1.5f;
    float attackTimer;
    public GameObject attackPrefab;
    GameObject attack;

    // health
    public float healthMax = 2;
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
            if (attackTimer > attackDelay && attack == null)
            {
                attack = Instantiate(attackPrefab);
                attack.transform.position = transform.position + (attackDir * attackSpacing);
                attack.transform.right = attackDir;
                velocity += attackDir * (speed * 2.5f);

                // Checking if player gets hit
                if (player.GetComponent<Renderer>().bounds.Intersects(attack.GetComponent<Renderer>().bounds))
                    player.GetComponent<PlayerScript>().GetHit();
            }

            // Reset the attack timer
            if (attackTimer >= attackTimerMax)
            {
                Destroy(attack);
                attackTimer = 0;
                attacking = false;
            }
        }

        // Carry out the math
        transform.position += velocity;
        direction = Vector3.zero;
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
}