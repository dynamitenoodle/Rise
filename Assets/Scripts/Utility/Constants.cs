﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{

    //Scene GameObject names
    public const string GAMEOBJECT_NAME_WAVEMANAGER = "WaveManager";
    public const string GAMEOBJECT_NAME_ENVIRONMENTMANAGER = "EnvironmentManager";
    public const string GAMEOBJECT_NAME_CANVAS = "Canvas";


    //tags
    public const string TAG_PLAYER = "Player";
    public const string TAG_ENVIRONMENT_WATER = "Environment_Water";
    public const string TAG_ENVIRONMENT_FIRE = "Environment_Fire";
    public const string TAG_ENVIRONMENT_STEAM = "Environment_Steam";

    //Level Gen 
    public const int LEVELGENERATION_MIN_ROOM_DISTANCE = 5;
    public const int LEVELGEN_MAX_ROOM_LOOPS = 25;
    public const int LEVELGEN_BOSS_ROOM_MAX_LOOPS = 1000;

    //wave gen
    public const float WAVEGEN_SAFE_ZONE = 15f;
    public const float WAVEGEN_SPAWN_ZONE = 30f; // this needs to be higher than the WAVEGEN_SAFE_ZONE number

    //camera
    public const float CAMERA_MAX_MOVE_DISTANCE = 1.5f;

    //BulletScript.cs constants
    public const float BULLET_DEFAULT_MIN_SCALE = 0.2f;

    //ability constants

    public const float ABILITY_RANGED_ATTACK_DEFAULT = 0.1f;

    //environment constants
    public const float ENVIRONMENT_FIRE_LIFE_SPAN = 20f;
    public const float ENVIRONMENT_STEAM_CREATE_RADIUS_MULTIPLIER = 2f;
    public const float ENVIRONMENT_UPDATE_TIME = 0.2f;

    //UI constants
    public const float UI_ABILITY_COOLDOWN_UPDATE_TIME = 0.1f;
}
