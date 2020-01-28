using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // attributes
    public float maxSpeed = .1f;
    public float speed = .02f;
    public float friction = .9f;
    public float sprintMultiplier = 1.5f;
    Vector3 velocity;
    Vector3 direction;

    // health stuffs
    public float healthMax;
    float health;
    bool invul;
    public float hitTimerMax;
    float hitTimer;

    // Wall collisions
    public List<GameObject> walls;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;
        health = healthMax;
        invul = false;

        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            walls.Add(wall);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Input check
        if (Input.GetKey(KeyCode.W))
            direction.y += 1;
        if (Input.GetKey(KeyCode.S))
            direction.y -= 1;
        if (Input.GetKey(KeyCode.D))
            direction.x += 1;
        if (Input.GetKey(KeyCode.A))
            direction.x -= 1;

        // Slowdown if nothing
        if (direction == Vector3.zero)
            velocity *= friction;

        WallCheck();

        velocity += (direction * speed);

        // Sprinting
        if (Input.GetKey(KeyCode.LeftShift))
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed * sprintMultiplier);
        else
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Carry out the math
        transform.position += velocity;
        direction = Vector3.zero;

        if (invul)
        {
            Flicker();
        }
    }

    // Called by other scripts to hit the player
    public void GetHit()
    {
        if (!invul)
        {
            invul = true;
            Debug.Log("Player was hit!");
            health--;

            if (health < 0)
                Destroy(gameObject);
        }
    }

    // Flickering if hit
    public void Flicker()
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

    void WallCheck()
    {
        /*
        List<GameObject> closeWalls = new List<GameObject>();
        closeWalls.Add(walls[0]);
        float closeDis = Vector3.Distance(closeWalls[0].transform.position, transform.position);
        foreach (GameObject wall in walls)
        {
            float newDis = Vector3.Distance(wall.transform.position, transform.position);
            if (newDis < closeDis)
            {
                closeWalls = new List<GameObject>();
                closeWalls.Add(wall);
                closeDis = newDis;
            }
            else if (newDis == closeDis && wall != walls[0])
            {
                closeWalls.Add(wall);
            }
        }
        */

        foreach (GameObject wall in walls)
        {
            if (GetComponent<Renderer>().bounds.Intersects(wall.GetComponent<Renderer>().bounds))
            {
                // Make an easier variable
                Vector3 wallPos = wall.transform.position;

                // Calculate the bounding boxes of the player and the wall
                float playerRight = transform.position.x + GetComponent<Renderer>().bounds.extents.x;
                float playerLeft = transform.position.x - GetComponent<Renderer>().bounds.extents.x;
                float playerTop = transform.position.y + GetComponent<Renderer>().bounds.extents.y;
                float playerBot = transform.position.y - GetComponent<Renderer>().bounds.extents.y;

                float wallRight = wallPos.x + wall.GetComponent<Renderer>().bounds.extents.x;
                float wallLeft = wallPos.x - wall.GetComponent<Renderer>().bounds.extents.x;
                float wallTop = wallPos.y + wall.GetComponent<Renderer>().bounds.extents.y;
                float wallBot = wallPos.y - wall.GetComponent<Renderer>().bounds.extents.y;

                // Set a distance to check walls (the number here works well with 1x1 boxes)
                float dis = .08f;

                // Checking which side to stop the player
                // Right
                if ((Mathf.Abs(playerRight - wallLeft) < dis) && transform.position.x < wallPos.x)
                {
                    if (direction.x > 0)
                        direction.x = 0;
                    if (velocity.x > 0)
                        velocity.x = 0;
                }
                // Left
                if (Mathf.Abs(playerLeft - wallRight) < dis && transform.position.x > wallPos.x)
                {
                    if (direction.x < 0)
                        direction.x = 0;
                    if (velocity.x < 0)
                        velocity.x = 0;
                }
                // Up
                if (Mathf.Abs(playerTop - wallBot) < dis && transform.position.y < wallPos.y)
                {
                    if (direction.y > 0)
                        direction.y = 0;
                    if (velocity.y > 0)
                        velocity.y = 0;
                }
                // Down
                if (Mathf.Abs(playerBot - wallTop) < dis && transform.position.y > wallPos.y)
                {
                    if (direction.y < 0)
                        direction.y = 0;
                    if (velocity.y < 0)
                        velocity.y = 0;
                }
            }
        }
    }
}