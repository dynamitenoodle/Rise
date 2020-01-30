﻿using System.Collections;
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
        InputCheck();

        // Slowdown if nothing
        if (direction == Vector3.zero)
            velocity *= friction;

        if (velocity.magnitude < 0.008f)
            velocity = Vector3.zero;

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

    // Checks if the player is colliding with walls
    void WallCheck()
    {
        // Checking all the wall collisions
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

                /* 
                 * Checking which side to stop the player
                 * The fist check looks to see if the bounding boxes sides are within a certain distance.
                */

                Vector3 fixedPos = transform.position;

                Vector3 playerToWall = Vector3.Normalize(wallPos - transform.position);
                Vector3 right = new Vector3(1, 0);
                float angle = Vector3.Angle(playerToWall, right);
                Debug.Log(angle);

                // Right
                if (Mathf.Abs(playerRight - wallLeft) < dis && (angle > 0 && angle < 45f))
                {
                    if (direction.x > 0)
                        direction.x = 0;
                    if (velocity.x > 0)
                        velocity.x = 0;

                    fixedPos.x = fixedPos.x - (Mathf.Abs(playerRight - wallLeft));
                }

                // Left
                if (Mathf.Abs(playerLeft - wallRight) < dis && (angle > 135f && angle < 225f))
                {
                    if (direction.x < 0)
                        direction.x = 0;
                    if (velocity.x < 0)
                        velocity.x = 0;

                    fixedPos.x = fixedPos.x + (Mathf.Abs(playerLeft - wallRight));
                }

                // Up
                if (Mathf.Abs(playerTop - wallBot) < dis && (angle > 45f && angle < 135f))
                { 
                    if (direction.y > 0)
                        direction.y = 0;
                    if (velocity.y > 0)
                        velocity.y = 0;

                    fixedPos.y = fixedPos.y - (Mathf.Abs(playerTop - wallBot));
                }

                // Down
                if (Mathf.Abs(playerBot - wallTop) < dis && (angle > 45f && angle < 135f))
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
    }
    
    // Checks the inputs
    void InputCheck()
    {
        if (Input.GetKey(KeyCode.W))
            direction.y += 1;
        if (Input.GetKey(KeyCode.S))
            direction.y -= 1;
        if (Input.GetKey(KeyCode.D))
            direction.x += 1;
        if (Input.GetKey(KeyCode.A))
            direction.x -= 1;

        // If nothing is pressed, get rid of the direction
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            direction.y = 0;
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            direction.x = 0;
    }
}