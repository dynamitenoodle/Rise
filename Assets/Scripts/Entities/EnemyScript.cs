using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // attributes
    public enum EnemySpawnType { common, uncommon, rare, special }
    public EnemySpawnType enemySpawnType;
    public enum EnemyAttackStyle { melee, ranged, boss }
    public EnemyAttackStyle enemyAttackStyle;
    float maxSpeed;
    float speed;
    float friction;
    float detectionDistance; //change this to reasonable number
    Vector3 velocity;
    Vector3 direction;
    GameObject player;

    // attack stuff
    [SerializeField] List<Attack> attacks;
    bool attacking;
    int attackRoll;
    float attackTimer;
    GameObject attackGO;
    bool fired = false;
    Vector3 attackDir;
    int shotNum;
    float globalAttackTimer;
    float minTimeBtwnAttacks = 2.0f;
    bool speenDir = false;

    // health
    float healthMax = 2;
    float health;

    WaveManager waveManager;

    // Graph stuff
    Graph graph;
    Node node;
    Node prevNode;
    int roomNum;
    bool chasingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
        graph = GameObject.Find("Graph").GetComponent<Graph>();
        velocity = Vector3.zero;
        health = healthMax;

        attackRoll = -1;
        shotNum = 0;
        node = graph.NearestNode(transform.position);
        roomNum = node.roomNum;
        chasingPlayer = false;

        // setting constant values
        SetConstants();
}

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            Destroy(gameObject);

        // If we don't have an attack selected
        if (attackRoll == -1)
        {
            PickAttack();
        }
        else
        {
            // Checks to see if we are in the same room as the player, and that our distance from them is within the range
            if (roomNum == player.GetComponent<PlayerScript>().Node.roomNum)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.transform.position - transform.position), detectionDistance);

                if (hit.collider != null && hit.collider.gameObject.tag == "Player")
                {
                    AttackUpdate();
                    node = graph.NearestNode(transform.position);
                }
                else if (!attacking)
                {
                    FollowNode();
                }
            }
            else if (!attacking)
                FollowNode();

            else if (attacking)
                AttackUpdate();
        }

        ApplyVelocity();
    }

    // Setting the variables that are constants
    void SetConstants()
    {
        if (enemyAttackStyle == EnemyAttackStyle.melee)
        {
            maxSpeed = Constants.ENEMY_MELEE_MAXSPEED;
            speed = Constants.ENEMY_MELEE_SPEED;
            friction = Constants.ENEMY_MELEE_FRICTION;
            detectionDistance = Constants.ENEMY_MELEE_DETECTION_DISTANCE;
            minTimeBtwnAttacks = Constants.ENEMY_MELEE_MINIMUM_TIME_BETWEEN_ATTACKS;
            healthMax = Constants.ENEMY_MELEE_HEALTH_MAX;
        }

        else if (enemyAttackStyle == EnemyAttackStyle.ranged)
        {
            maxSpeed = Constants.ENEMY_RANGED_MAXSPEED;
            speed = Constants.ENEMY_RANGED_SPEED;
            friction = Constants.ENEMY_RANGED_FRICTION;
            detectionDistance = Constants.ENEMY_RANGED_DETECTION_DISTANCE;
            minTimeBtwnAttacks = Constants.ENEMY_RANGED_MINIMUM_TIME_BETWEEN_ATTACKS;
            healthMax = Constants.ENEMY_RANGED_HEALTH_MAX;
        }

        else if (enemyAttackStyle == EnemyAttackStyle.boss)
        {
            maxSpeed = Constants.ENEMY_BOSS_MAXSPEED;
            speed = Constants.ENEMY_BOSS_SPEED;
            friction = Constants.ENEMY_BOSS_FRICTION;
            detectionDistance = Constants.ENEMY_BOSS_DETECTION_DISTANCE;
            minTimeBtwnAttacks = Constants.ENEMY_BOSS_MINIMUM_TIME_BETWEEN_ATTACKS;
            healthMax = Constants.ENEMY_BOSS_HEALTH_MAX;
        }
    }

    #region Attack
    // Enemy AttackUpdate
    void AttackUpdate()
    {
        chasingPlayer = true;
        float playerDis = (player.transform.position - transform.position).magnitude;

        // Where is PLAYER
        if (!AttackDisCheck(playerDis) && !attacking)
        {
            direction = (player.transform.position - transform.position).normalized;
            CannonSet(direction);
        }

        // Attacking stuff
        if (attacking)
        {
            attackTimer += Time.deltaTime;

            // SPEEEEN calculation
            if (attackGO != null && attacks[attackRoll].speen)
            {
                if (speenDir)
                    attackGO.transform.Rotate(Vector3.forward * attacks[attackRoll].speenSpeed * Time.deltaTime);
                else
                    attackGO.transform.Rotate(Vector3.back * attacks[attackRoll].speenSpeed * Time.deltaTime);
            }

            CannonSet(attackDir);

            // Create the attackGO when it is time
            if (CanAttackCheck())
            {
                Attack();
            }

            // Reset the attack timer
            if (attackTimer >= attacks[attackRoll].attackTimerMax &&
                ((attacks[attackRoll].shotNumMax == 1 || shotNum >= attacks[attackRoll].shotNumMax)
                || attackTimer >= attacks[attackRoll].attackTimerMax + attacks[attackRoll].attackDelay))
            {
                if (attacks[attackRoll].isMelee)
                    Destroy(attackGO);

                else
                    fired = false;

                attackTimer = 0;

                // Checks if we should shoot more
                if (shotNum < attacks[attackRoll].shotNumMax && attacks[attackRoll].shotNumMax != 1)
                    shotNum++;

                else
                {
                    attacking = false;
                    shotNum = 0;
                    attackRoll = -1;
                }
            }
        }
    }

    // Selects an attack for the enemy
    void PickAttack()
    {
        globalAttackTimer += Time.deltaTime;

        // If the enemy can attack again
        if (globalAttackTimer > minTimeBtwnAttacks)
        {
            attackRoll = Random.Range(0, attacks.Count);
            globalAttackTimer = 0;

            // pick spin direction for the attack
            switch (Random.Range(0, 2))
            {
                case 0:
                    speenDir = false;
                    break;
                default:
                    speenDir = true;
                    break;
            }
        }
    }

    // The check for if player is within attack distance
    bool AttackDisCheck(float playerDis)
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

    // Method to see if the enemy can attack the player
    bool CanAttackCheck()
    {
        if (((attacks[attackRoll].isMelee && attackGO == null) || !attacks[attackRoll].isMelee))
        {
            if ((attackTimer > attacks[attackRoll].attackDelay && attacks[attackRoll].shotNumMax == 1))
                return true;

            if (attackTimer > attacks[attackRoll].attackDelay + attacks[attackRoll].increasedDelay && attacks[attackRoll].shotNumMax > 1 && shotNum == 0)
                return true;

            if (attackTimer > attacks[attackRoll].attackDelay && attacks[attackRoll].shotNumMax > 1 && shotNum > 0)
                return true;
        }
        return false;
    }

    // Executing the attack
    void Attack()
    {
        if (attacks[attackRoll].isMelee)
        {
            // Melee Attack
            attackGO = Instantiate(attacks[attackRoll].attackPrefab);
            attackGO.transform.position = transform.position + (attackDir * attacks[attackRoll].attackSpacing);
            attackGO.transform.right = attackDir;

            velocity += attackDir * (speed * attacks[attackRoll].kickBack);
        }

        else if (!fired && !attacks[attackRoll].isMelee)
        {
            // Range Attack
            attackGO = Instantiate(attacks[attackRoll].attackPrefab);
            attackGO.transform.position = transform.position + (attackDir * attacks[attackRoll].attackSpacing);
            attackGO.transform.right = attackDir;
            attackGO.GetComponent<BulletScript>().SetAttributes(attackDir, attacks[attackRoll].bulletSpeed);

            velocity += attackDir * (speed * attacks[attackRoll].kickBack);

            fired = true;
        }
    }

    // Sets the cannon angle
    void CannonSet(Vector3 dir)
    {
        if (attackRoll != -1 && (attacks.Count != 1 || !attacks[0].isMelee))
        {
            float cannonAngle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90f;
            gameObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, cannonAngle);
        }
    }
    #endregion

    // Called by other scripts to hit the player
    public void GetHit(Vector3 knockback, float knockbackAmt)
    {
        health--;

        velocity -= knockback * knockbackAmt;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        if (health - 1 < 0)
        {
            if (attackGO != null)
                Destroy(attackGO);

            waveManager.DestroyEnemy(this.gameObject);
            Destroy(gameObject);
        }

    }

    // Apply the velocity to the enemy
    void ApplyVelocity()
    {
        // Slowdown if nothing
        if (direction == Vector3.zero)
            velocity *= friction;

        velocity += (direction * speed);

        if (attackRoll != -1)
        {
            if (attacks[attackRoll].kickBack < 5)
                velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            else
                velocity = Vector3.ClampMagnitude(velocity, maxSpeed * attacks[attackRoll].kickBack);
        }

        // Carry out the math
        transform.position += velocity;
        direction = Vector3.zero;
    }

    // Sets the enemy node
    public void SetNode(Node nd)
    {
        node = nd;
    }

    // Following the nodes
    void FollowNode()
    {
        if (Vector3.Distance(node.pos, transform.position) < .2f)
        {
            roomNum = node.roomNum;
            Node tempNode = node;
            node = graph.GetNextNode(node, prevNode);
            prevNode = tempNode;
        }

        direction = Vector3.Normalize(node.pos - transform.position);
        CannonSet(direction);
        chasingPlayer = false;

    }

    // Drawing the gizmos
    void OnDrawGizmos()
    {
        if (!chasingPlayer)
        {
            Gizmos.color = new Color(50, 150, 50, 0.5f);
            Gizmos.DrawLine(transform.position, node.pos);
        }
        else
        {
            Gizmos.color = new Color(150, 50, 50, 0.5f);
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }
}