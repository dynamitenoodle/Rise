using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{

    //Scene GameObject names
    public const string GAMEOBJECT_NAME_LEVELMANAGER = "LevelManager";
    public const string GAMEOBJECT_NAME_ENVIRONMENTMANAGER = "EnvironmentManager";
    public const string GAMEOBJECT_NAME_CANVAS = "Canvas";


    //tags
    public const string TAG_PLAYER = "Player";
    public const string TAG_ENVIRONMENT_WATER = "Environment_Water";
    public const string TAG_ENVIRONMENT_FIRE = "Environment_Fire";
    public const string TAG_ENVIRONMENT_STEAM = "Environment_Steam";

    //Level Gen 
    public const int LEVELGENERATION_MIN_ROOM_DISTANCE = 5;
    public const int LEVELGEN_MAX_ROOM_LOOPS = 40;
    public const int LEVELGEN_BOSS_ROOM_MAX_LOOPS = 5000;

    //wave gen
    public const float WAVEGEN_SAFE_ZONE = 15f;
    public const float WAVEGEN_SPAWN_ZONE = 30f; // this needs to be higher than the WAVEGEN_SAFE_ZONE number

    //camera
    public const float CAMERA_MAX_MOVE_DISTANCE = 1.5f;

    //BulletScript.cs constants
    public const float BULLET_DEFAULT_MIN_SCALE = 0.2f;

    //ability constants

    public const float ABILITY_RANGED_ATTACK_DEFAULT_SPEED = 0.1f;

    //environment constants
    public const float ENVIRONMENT_FIRE_LIFE_SPAN = 20f;
    public const float ENVIRONMENT_STEAM_CREATE_RADIUS_MULTIPLIER = 2f;
    public const float ENVIRONMENT_UPDATE_TIME = 0.2f;
    public const float ENVIRONMENT_ADD_RIGIDBODY_AFTER_SECONDS = 0.1f;

    //UI constants
    public const float UI_ABILITY_COOLDOWN_UPDATE_TIME = 0.1f;

    // Enemy constants
    // Melee
    public const float ENEMY_MELEE_MAXSPEED = .04f;
    public const float ENEMY_MELEE_SPEED = .005f;
    public const float ENEMY_MELEE_FRICTION = .9f;
    public const float ENEMY_MELEE_DETECTION_DISTANCE = 15f;
    public const float ENEMY_MELEE_MINIMUM_TIME_BETWEEN_ATTACKS = 2.0f;
    public const float ENEMY_MELEE_HEALTH_MAX = 2f;
    // Ranged
    public const float ENEMY_RANGED_MAXSPEED = .035f;
    public const float ENEMY_RANGED_SPEED = .005f;
    public const float ENEMY_RANGED_FRICTION = .9f;
    public const float ENEMY_RANGED_DETECTION_DISTANCE = 15f;
    public const float ENEMY_RANGED_MINIMUM_TIME_BETWEEN_ATTACKS = 2.0f;
    public const float ENEMY_RANGED_HEALTH_MAX = 3f;
    // Boss
    public const float ENEMY_BOSS_MAXSPEED = .025f;
    public const float ENEMY_BOSS_SPEED = .001f;
    public const float ENEMY_BOSS_FRICTION = .9f;
    public const float ENEMY_BOSS_DETECTION_DISTANCE = 30f;
    public const float ENEMY_BOSS_MINIMUM_TIME_BETWEEN_ATTACKS = 2.0f;
    public const float ENEMY_BOSS_HEALTH_MAX = 10f;
    //Resources constants
    public const string RESOURCES_ABILITIES = "Abilities";
    public const string RESOURCES_PLAYER = "Entities/Player";
}
