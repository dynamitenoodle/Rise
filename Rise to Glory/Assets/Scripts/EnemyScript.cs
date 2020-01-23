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

    bool attacking;
    Vector3 attackDir;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        velocity = Vector3.zero;
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

        // Carry out the math
        transform.position += velocity;
        direction = Vector3.zero;
    }

    bool AttackCheck(float playerDis)
    {
        if (attackRange > playerDis)
        {
            attacking = true;
            attackDir = direction;
            return true;
        }

        return false;
    }
}
