using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //attributes
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float speed;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "EnemyAttack")
            player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (direction * speed);

        // Checking if player gets hit
        if (gameObject.tag == "EnemyAttack" && player.GetComponent<Collider2D>().bounds.Intersects(GetComponent<Collider2D>().bounds))
        {
            player.GetComponent<PlayerScript>().GetHit();
            Destroy(gameObject);
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (gameObject.tag == "PlayerAttack" && enemy.GetComponent<Collider2D>().bounds.Intersects(GetComponent<Collider2D>().bounds))
            {
                enemy.GetComponent<EnemyScript>().GetHit(Vector3.zero, 1);
                Destroy(gameObject);
            }
        }
        CollisionCheck();
    }

    public void SetAttributes(Vector3 dir, float spd)
    {
        direction = dir;
        speed = spd;
    }

    void CollisionCheck()
    {
        foreach (Collider2D col in FindObjectsOfType<Collider2D>())
        {
            // if the col isn't this object
            if (col.gameObject != gameObject && col.gameObject.tag == "Wall")
            {
                if (col.bounds.Intersects(GetComponent<Collider2D>().bounds))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
