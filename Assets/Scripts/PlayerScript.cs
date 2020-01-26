using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float maxSpeed = .1f;
    public float speed = .02f;
    public float friction = .9f;
    public float sprintMultiplier = 1.5f;
    Vector3 velocity;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;

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
    }
}
