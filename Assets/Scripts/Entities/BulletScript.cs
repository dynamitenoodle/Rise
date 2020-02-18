using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //attributes
    [HideInInspector] public Vector2 direction;
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
        transform.position += (Vector3)(direction * speed);

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
                enemy.GetComponent<EnemyScript>().GetHit(Vector2.zero, 1);
                Destroy(gameObject);
            }
        }
    }

    public void SetAttributes(Vector2 dir, float spd)
    {
        direction = dir;
        speed = spd;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("test");
        // if the col isn't this object
        if (col.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
