using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDescriber : MonoBehaviour
{
    public enum DoorLocation
    {
        North,
        South,
        East,
        West
    }

    public DoorLocation doorLocation = DoorLocation.North;
}
