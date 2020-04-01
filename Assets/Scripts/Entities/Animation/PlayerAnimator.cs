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
    private Sprite[] playerWalkSprites;

    PlayerScript playerScript;

    [Range(0.1f, 2.0f)][SerializeField]
    private float walkCycleSpeed;
    private float startAnimationTime;
    private int animationState;

    private void Start()
    {
        startAnimationTime = 0;
        animationState = 0;
        playerScript = GetComponent<PlayerScript>();

        playerBodySprites = new Sprite[8];
        playerBodySprites[0] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_back");
        playerBodySprites[1] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_front");
        playerBodySprites[2] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_left");
        playerBodySprites[3] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_right");
        playerBodySprites[4] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_back-left");
        playerBodySprites[5] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_back-right");
        playerBodySprites[6] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_front-left");
        playerBodySprites[7] = Resources.Load<Sprite>($"{Constants.RESOURCES_PLAYER}/player_front-right");


        string[] loadWalkCycles =
        {
            $"{Constants.RESOURCES_PLAYER}/Animations/player_back_w_1",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_back_w_2",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_front_w_1",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_front_w_2",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_left_w_1",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_left_w_2",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_right_w_1",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_right_w_2",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_back-left_w_1",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_back-left_w_2",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_back-right_w_1",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_back-right_w_2",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_front-left_w_1",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_front-left_w_2",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_front-right_w_1",
            $"{Constants.RESOURCES_PLAYER}/Animations/player_front-right_w_2"
        };

        playerWalkSprites = loadSprites(loadWalkCycles);

    }

    private Sprite[] loadSprites(string[] fileNames)
    {
        Sprite[] sprites = new Sprite[fileNames.Length];
        for (int i = 0; i < fileNames.Length; i++)
        {
            sprites[i] = Resources.Load<Sprite>(fileNames[i]);
        }
        return sprites;
    }

    void Update()
    {

        int playerBodyIndex = GetLookingDirection();

        if (playerScript.moving)
        {
            /*
            if (Time.time - startAnimationTime >= walkCycleSpeed)
            {
                startAnimationTime = Time.time;
                animationState++;
                if (animationState > 2) { animationState = 0; }
            }

            Sprite curSprite = null;
            switch (animationState)
            {
                case 0:
                    curSprite = playerBodySprites[playerBodyIndex];
                    break;
                case 1:
                    curSprite = playerWalkSprites[playerBodyIndex * 2];
                    break;
                case 2:
                    curSprite = playerWalkSprites[(playerBodyIndex * 2) + 1];
                    break;
                default:
                    curSprite = playerBodySprites[playerBodyIndex];
                    break;
            }
            this.gameObject.GetComponent<SpriteRenderer>().sprite = curSprite;
            */
        }
        else
        {
            //this.gameObject.GetComponent<SpriteRenderer>().sprite = playerBodySprites[playerBodyIndex];
        }
        this.gameObject.GetComponent<SpriteRenderer>().sprite = playerBodySprites[playerBodyIndex];

    }

    private int GetLookingDirection()
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

        return playerBodyIndex;
    }
}
