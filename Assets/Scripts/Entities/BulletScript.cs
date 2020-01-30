using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //attributes
    public Vector3 direction;
    public float speed;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (direction * speed);

        // Checking if player gets hit
        if (player.GetComponent<Renderer>().bounds.Intersects(GetComponent<Renderer>().bounds))
        {
            player.GetComponent<PlayerScript>().GetHit();
            Destroy(gameObject);
        }
    }

    public void SetAttributes(Vector3 dir, float spd)
    {
        direction = dir;
        speed = spd;
    }
}
