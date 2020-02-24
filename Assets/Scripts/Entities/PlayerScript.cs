using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // attributes
    [SerializeField] float maxSpeed = .1f;
    [SerializeField] float speed = .02f;
    [SerializeField] float friction = .9f;
    Vector3 velocity;
    Vector3 direction;

    // health stuffs
    [SerializeField] float healthMax = 3;
    float health;
    bool invul;
    [SerializeField] float hitTimerMax = 2;
    float hitTimer;

    // Wall collisions
    List<GameObject> walls;

    // ATTACK STUFF
    public Attack melee;
    bool attacking;
    float attackTimer;
    GameObject attackGO;
    bool fired = false;
    Vector3 attackDir;
    float globalAttackTimer;
    [SerializeField] float minTimeBtwnAttacks = 0.3f;
    bool speenDir = false;
    Vector3 lastDir;

    //list of abilities
    Ability[] abilities = new Ability[5];

    // room stuff
    Node node;
    public Node Node { get { return node; } }

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;
        health = healthMax;
        invul = false;

        walls = new List<GameObject>();

        attackTimer = globalAttackTimer;
        abilities[0] = gameObject.AddComponent<Ability_MagicBlast>();
    }

    // Update is called once per frame
    void Update()
    {
        InputCheck();
        WallCheck();
        ApplyVelocity();
        Flicker();
        Attack();
        
    }

    // Called by other scripts to hit the player
    public void GetHit()
    {
        if (!invul)
        {
            invul = true;
            health--;

            if (health - 1 < 0)
                Application.Quit();
        }
    }

    // Flickering if hit
    public void Flicker()
    {
        if (invul)
        {
            hitTimer += Time.deltaTime;

            //Flicker effect
            Color tempColor = GetComponent<SpriteRenderer>().color;

            if ((hitTimer * 100) % 20 > 10)
                tempColor.a = .6f;
            else
                tempColor.a = 1f;

            GetComponent<SpriteRenderer>().color = tempColor;

            if (hitTimer > hitTimerMax)
            {
                hitTimer = 0;
                invul = false;
                tempColor.a = 1f;
                GetComponent<SpriteRenderer>().color = tempColor;
            }
        }
    }

    // Checks if the player is colliding with walls
    void WallCheck()
    {
        if (walls != null)
        {
            // Checking all the wall collisions
            foreach (GameObject wall in walls)
            {
                if (wall != null)
                {
                    if (GetComponent<Collider2D>().bounds.Intersects(wall.GetComponent<Collider2D>().bounds))
                    {
                        // Make an easier variable
                        Vector3 wallPos = wall.transform.position;

                        // Calculate the bounding boxes of the player and the wall
                        float playerRight = transform.position.x + GetComponent<Collider2D>().bounds.extents.x;
                        float playerLeft = transform.position.x - GetComponent<Collider2D>().bounds.extents.x;
                        float playerTop = transform.position.y + GetComponent<Collider2D>().bounds.extents.y;
                        float playerBot = transform.position.y - GetComponent<Collider2D>().bounds.extents.y;

                        float wallRight = wallPos.x + wall.GetComponent<Collider2D>().bounds.extents.x;
                        float wallLeft = wallPos.x - wall.GetComponent<Collider2D>().bounds.extents.x;
                        float wallTop = wallPos.y + wall.GetComponent<Collider2D>().bounds.extents.y;
                        float wallBot = wallPos.y - wall.GetComponent<Collider2D>().bounds.extents.y;

                        // Set a distance to check walls (the number here works well with 1x1 boxes)
                        float dis = .08f;

                        /* 
                         * Checking which side to stop the player
                         * The fist check looks to see if the bounding boxes sides are within a certain distance.
                         * The second check looks to see if the wall is on the correct side to check
                        */

                        Vector3 fixedPos = transform.position;

                        Vector3 playerToWall = Vector3.Normalize(wallPos - transform.position);
                        Vector3 right = new Vector3(1, 0);
                        float angle = Vector3.Angle(playerToWall, right);

                        // Right
                        if (Mathf.Abs(playerRight - wallLeft) < dis && (angle >= 0 && angle <= 45f))
                        {
                            if (direction.x > 0)
                                direction.x = 0;
                            if (velocity.x > 0)
                                velocity.x = 0;

                            fixedPos.x = fixedPos.x - (Mathf.Abs(playerRight - wallLeft));
                        }

                        // Left
                        if (Mathf.Abs(playerLeft - wallRight) < dis && (angle >= 135f && angle <= 225f))
                        {
                            if (direction.x < 0)
                                direction.x = 0;
                            if (velocity.x < 0)
                                velocity.x = 0;

                            fixedPos.x = fixedPos.x + (Mathf.Abs(playerLeft - wallRight));
                        }

                        // Up
                        if (Mathf.Abs(playerTop - wallBot) < dis && (angle >= 45f && angle <= 135f))
                        {
                            if (direction.y > 0)
                                direction.y = 0;
                            if (velocity.y > 0)
                                velocity.y = 0;

                            fixedPos.y = fixedPos.y - (Mathf.Abs(playerTop - wallBot));
                        }

                        // Down
                        if (Mathf.Abs(playerBot - wallTop) < dis && (angle >= 45f && angle <= 135f))
                        {
                            if (direction.y < 0)
                                direction.y = 0;
                            if (velocity.y < 0)
                                velocity.y = 0;

                            fixedPos.y = fixedPos.y + (Mathf.Abs(playerBot - wallTop));
                        }

                        transform.position = fixedPos;
                    }
                }
                else
                {
                    SetWalls();
                    break;
                }
            }
        }
    }

    // Checks the inputs
    void InputCheck()
    {
        if (!attacking)
        {
            if (Input.GetKey(KeyCode.W))
                direction.y += 1;
            if (Input.GetKey(KeyCode.S))
                direction.y -= 1;
            if (Input.GetKey(KeyCode.D))
                direction.x += 1;
            if (Input.GetKey(KeyCode.A))
                direction.x -= 1;
        }
        // If nothing is pressed, get rid of the direction
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            direction.y = 0;
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            direction.x = 0;

        // If the player attacks
        if (Input.GetMouseButtonDown(0) && attackTimer > globalAttackTimer && attackGO == null)
        {
            //Attack();
            abilities[0].Action();
            attackTimer = 0;
        }
    }

    // Applies the velocity to the position
    void ApplyVelocity()
    {
        // Slowdown if nothing
        if (direction == Vector3.zero)
            velocity *= friction;
        else if (direction.x == 0)
            velocity.x *= friction;
        else if (direction.y == 0)
            velocity.y *= friction;

        if (velocity.magnitude < 0.008f)
            velocity = Vector3.zero;

        velocity += (direction * speed);
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += velocity;

        if (direction != Vector3.zero)
        {
            lastDir = direction;
            direction = Vector3.zero;
        }
    }

    // Checks if the player should be attacking
    void Attack()
    {
        // Timers
        attackTimer += Time.deltaTime;

        // Reset the attack timer
        if (attackTimer >= melee.attackTimerMax)
        {
            if (melee.isMelee)
                Destroy(attackGO);

            else
                fired = false;

            attackTimer = 0;
            attacking = false;
        }

        
    }

    // Sets the walls for the player to use for collision after they are generated
    public void SetWalls()
    {
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            walls.Add(wall);
        }
    }

    // Sets the player node
    public void SetNode(Node nd)
    {
        node = nd;
    }
}