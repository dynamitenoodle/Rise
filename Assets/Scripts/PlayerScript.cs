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

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;
        health = healthMax;
        invul = false;
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
}