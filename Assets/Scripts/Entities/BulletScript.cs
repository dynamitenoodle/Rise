using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //attributes
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float speed;
    [HideInInspector] public delegate void OnDeathCallback(GameObject obj);
    [HideInInspector] public OnDeathCallback onDeathCallback = null;
    GameObject player;

    private float bulletStartTime = -1;
    private float bulletDeathTime = -1;
    private bool bulletDoesFade = false;
    private bool bulletDoesScale = false;

    public Vector2 maxScale;
    public Vector2 minScale;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "EnemyAttack")
            player = GameObject.FindGameObjectWithTag("Player");


        maxScale = this.transform.localScale;
        minScale = maxScale * Constants.BULLET_DEFAULT_MIN_SCALE;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)(direction * speed);

        CheckHit();
        if (bulletDoesFade || bulletDoesScale)
            AdjustBullet();
        if (bulletDeathTime != -1)
        {
            if (Time.time >= bulletDeathTime)
            {
                DestroyBullet();
            }
        }
    }

    private void AdjustBullet()
    {
        float time = (Time.time - bulletStartTime) / (bulletDeathTime - bulletStartTime);

        if (bulletDoesScale)
        {
            Vector2 curScale = Vector2.Lerp(maxScale, minScale, time);
            this.transform.localScale = curScale;
        }

        if (bulletDoesFade)
        {
            Color color = this.GetComponent<SpriteRenderer>().color;
            color.a = Mathf.Lerp(1, Constants.BULLET_DEFAULT_MIN_SCALE, time);
            this.GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void CheckHit()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (gameObject.tag == "PlayerAttack" && enemy.GetComponent<Collider2D>().bounds.Intersects(GetComponent<Collider2D>().bounds))
            {
                enemy.GetComponent<EnemyScript>().GetHit(Vector2.zero, 1);
            }
        }
    }

    public void SetAttributes(Vector2 dir, float spd)
    {
        direction = dir;
        speed = spd;
    }

    public void SetTimeout(float time, bool fade, bool scale)
    {
        bulletStartTime = Time.time;
        bulletDeathTime = bulletStartTime + time;

        bulletDoesFade = fade;
        bulletDoesScale = scale;
    }

    public void SetCallback(OnDeathCallback callBack)
    {
        onDeathCallback = callBack;
    }

    public void DestroyBullet()
    {
        onDeathCallback?.Invoke(this.gameObject);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // if the col isn't this object
        if (col.gameObject.tag == "Wall" || (gameObject.tag == "PlayerAttack" && col.gameObject.tag == "Enemy") || (gameObject.tag == "EnemyAttack" && col.gameObject.tag == "Player"))
            DestroyBullet();

        if (col.gameObject.tag == "Enemy" && gameObject.tag == "PlayerAttack")
            col.gameObject.GetComponent<EnemyScript>().GetHit(Vector2.zero, 2);
    }
}
