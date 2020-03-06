using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float verticalMouseOffset;
    [Range(0.0f, 10.0f)]
    public float horizontalMouseOffset;

    private Sprite[] playerBodySprites;

    private void Start()
    {
        playerBodySprites = new Sprite[8];
        playerBodySprites[0] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_back");
        playerBodySprites[1] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_front");
        playerBodySprites[2] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_left");
        playerBodySprites[3] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_right");
        playerBodySprites[4] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_back-left");
        playerBodySprites[5] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_back-right");
        playerBodySprites[6] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_front-left");
        playerBodySprites[7] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_front-right");

    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos = mousePos - (Vector2)(this.gameObject.transform.position);

        float verticalOffset = mousePos.y / verticalMouseOffset;
        float horrizontalOffset = mousePos.x / horizontalMouseOffset;

        int playerBodyIndex = -1;

        //back
        if ((horrizontalOffset > -1 && horrizontalOffset < 1) && verticalOffset > 0)
        {
            playerBodyIndex = 0;
        }
        //front
        else if ((horrizontalOffset > -1 && horrizontalOffset < 1) && verticalOffset < 0)
        {
            playerBodyIndex = 1;
        }
        //left
        else if ((verticalOffset > -1 && verticalOffset < 1) && horrizontalOffset < 0)
        {
            playerBodyIndex = 2;
        }
        //right
        else if ((verticalOffset > -1 && verticalOffset < 1) && horrizontalOffset > 0)
        {
            playerBodyIndex = 3;
        }
        //back-left
        else if (horrizontalOffset < -1 && verticalOffset > 1)
        {
            playerBodyIndex = 4;
        }
        //back-right
        else if (horrizontalOffset > 1 && verticalOffset > 1)
        {
            playerBodyIndex = 5;
        }
        //front-left
        else if (horrizontalOffset < -1 && verticalOffset < -1)
        {
            playerBodyIndex = 6;
        }
        //front right
        else if (horrizontalOffset > 1 && verticalOffset < -1)
        {
            playerBodyIndex = 7;
        }

        this.gameObject.GetComponent<SpriteRenderer>().sprite = playerBodySprites[playerBodyIndex];
    }
}
