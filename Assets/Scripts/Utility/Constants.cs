using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{

    //Scene GameObject names
    public const string GAMEOBJECT_NAME_WAVEMANAGER = "WaveManager";


    //Level Gen 
    public const int LEVELGENERATION_MIN_ROOM_DISTANCE = 5;
    public const int LEVELGEN_MAX_ROOM_LOOPS = 25;

    //wave gen
    public const float WAVEGEN_SAFE_ZONE = 15f;
    public const float WAVEGEN_SPAWN_ZONE = 30f; // this needs to be higher than the WAVEGEN_SAFE_ZONE number
    


}
