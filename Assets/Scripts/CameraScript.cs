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

        //where is mouse
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 moveDiff = (playerPos - mouseWorldPos) / 4;

        moveDiff = Vector3.ClampMagnitude(moveDiff, Constants.CAMERA_MAX_MOVE_DISTANCE);

        transform.position = playerPos - moveDiff;
    }
}
