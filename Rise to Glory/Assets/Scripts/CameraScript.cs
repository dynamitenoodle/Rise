using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Where is PLAYER
        Vector3 playerPos = player.transform.position;
        playerPos.z = transform.position.z;
        transform.position = playerPos;
    }
}
