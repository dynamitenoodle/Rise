using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    //public vars for adjustments
    public int minRoomSpawns;
    public int maxRoomSpawns;

    public List<GameObject> rooms;

    public Transform mapTransform;

    public GameObject tester;

    //class vars
    Helper helper;

    struct RoomSpawn
    {
        public int type;
        public Vector2 location;
        public int roomSize;
        public List<DoorDescriber> doors;
        public GameObject obj;
    }

    struct Hall
    {
        public Vector2 location;
        public int type;
    }


    // Start is called before the first frame update
    void Start()
    {
        helper = new Helper();
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        //spawn rooms
        Debug.Log("Generating rooms...");
        int numRooms = Random.Range(minRoomSpawns, maxRoomSpawns);
        Debug.Log($"NumRooms: {numRooms}");
        List<RoomSpawn> roomSpawns = SpawnRooms(numRooms);

        for (int i = 0; i < roomSpawns.Count; i++)
        {
            for (int j = 0; j < roomSpawns[i].doors.Count; j++)
            {
                if (roomSpawns[i].doors[j].doorOpen)
                {
                    GameObject testDoor = Instantiate(tester);
                    testDoor.transform.position = roomSpawns[i].location + roomSpawns[i].doors[j].location;
                }
            }
        }

        Debug.Log("Finished!");
    }

    //generates the locations to spawn preset rooms
    private List<RoomSpawn> SpawnRooms(int numRooms)
    {
        List<RoomSpawn> roomSpawns = new List<RoomSpawn>();

        //spawn first room
        roomSpawns.Add(GenerateRoom(0, 0));
        roomSpawns[0].obj.transform.position = roomSpawns[0].location;

        for (int i = 0; i < numRooms - 1; i++)
        {
            bool roomSpawnSuccess = true;
            int loopCount = 0;
            RoomSpawn roomSpawn;
            do
            {
                //pick room that already exists
                int roomPick = GetRandomValidRoom(roomSpawns);
                int doorPick = GetRandomValidDoor(roomSpawns[roomPick]);

                Vector2 doorPositionAdjust = GetDoorAdjust(roomSpawns[roomPick], doorPick);

                //find door of new room
                roomSpawn = GenerateRoom((int)roomSpawns[roomPick].location.x, (int)roomSpawns[roomPick].location.y);
                Vector2 roomMove = Vector2.zero;
                for (int j = 0; j < roomSpawn.doors.Count; j++)
                {
                    Vector2 doorAdjust = GetDoorAdjust(roomSpawn, j);
                    if (IsDoorInverse(doorPositionAdjust, doorAdjust))
                    {
                        Vector2 doorLoc = new Vector2(
                            roomSpawn.location.x + roomSpawn.doors[j].location.x,
                            roomSpawn.location.y + roomSpawn.doors[j].location.y);
                        Vector2 matchLoc = new Vector2(
                            roomSpawns[roomPick].location.x + roomSpawns[roomPick].doors[doorPick].location.x + doorPositionAdjust.x,
                            roomSpawns[roomPick].location.y + roomSpawns[roomPick].doors[doorPick].location.y + doorPositionAdjust.y);
                        Debug.Log($"matchLoc: {matchLoc}, doorLoc: {doorLoc}");
                        roomMove = matchLoc - doorLoc;

                        //remove used doors
                        roomSpawns[roomPick].doors[doorPick].doorOpen = false;
                        roomSpawn.doors[j].doorOpen = false;

                        
                        break;
                    }
                }

                roomSpawn.location += roomMove;
                roomSpawn.obj.transform.position = roomSpawn.location;

                //Debug.Log($"New room spawned moving it: {roomMove} ");
                Debug.Log($"Spawning room at loc: {roomSpawn.location} of type [{roomSpawn.type}] from room {roomPick}");

                loopCount++;
                if (loopCount >= 10)
                {
                    //force room spawn success as too many loops have occurred
                    roomSpawnSuccess = true;
                }
            } while (!roomSpawnSuccess);

            if (roomSpawn.type != -1)
            {
                roomSpawns.Add(roomSpawn);
                CheckOverlapDoors(roomSpawns);
            }
        }

        return roomSpawns;
    }

    private void CheckOverlapDoors(List<RoomSpawn> roomSpawns)
    {
        //room loop
        foreach (RoomSpawn room1 in roomSpawns)
        {
            foreach (RoomSpawn room2 in roomSpawns)
            {
                if (!room1.Equals(room2))
                {
                    //room1 loop through doors
                    for (int i1 = 0; i1 < room1.doors.Count; i1++)
                    {
                        for (int i2 = 0; i2 < room2.doors.Count; i2++)
                        {
                            float distance = helper.getDistance(room1.doors[i1].location + room1.location, room2.doors[i2].location + room2.location);
                            if (distance <= 1)
                            {
                                room1.doors[i1].doorOpen = false;
                                room2.doors[i2].doorOpen = false;
                            }
                        }
                    }
                }
            }
        }
    }

    private int GetRandomValidDoor(RoomSpawn roomSpawn)
    {
        List<int> validDoors = new List<int>();

        for (int i = 0; i < roomSpawn.doors.Count; i++)
        {
            if (roomSpawn.doors[i].doorOpen)
            {
                validDoors.Add(i);
            }
        }

        return validDoors[Random.Range(0, validDoors.Count - 1)];
    }

    private int GetRandomValidRoom(List<RoomSpawn> roomSpawns)
    {
        List<int> validRooms = new List<int>();

        for (int i = 0; i < roomSpawns.Count; i++)
        {
            int validDoors = 0;
            for (int j = 0; j < roomSpawns[i].doors.Count; j++)
            {
                if (roomSpawns[i].doors[j].doorOpen)
                {
                    validDoors++;
                }
            }

            if (validDoors != 0)
            {
                validRooms.Add(i);
            }
        }

        return validRooms[Random.Range(0, validRooms.Count - 1)];
    }

    private bool IsDoorInverse(Vector2 door, Vector2 door2)
    {
        if (door.x == 0)
        {
            return (door.y * -1 == door2.y);
        }
        else
        {
            return (door.x * -1 == door2.x);
        }
    }

    private Vector2 GetDoorAdjust(RoomSpawn room, int door)
    {
        DoorDescriber doorDescriber = room.doors[door];
        switch (doorDescriber.doorLocation)
        {
            case DoorDescriber.DoorLocation.North:
                return new Vector2(0, 1);
            case DoorDescriber.DoorLocation.South:
                return new Vector2(0, -1);
            case DoorDescriber.DoorLocation.East:
                return new Vector2(1, 0);
            case DoorDescriber.DoorLocation.West:
                return new Vector2(-1, 0);

        }
        return Vector2.zero;
    }

    //generates a random room with a given location to pass back to room spawning method
    private RoomSpawn GenerateRoom(int x, int y)
    {
        RoomSpawn roomSpawn = new RoomSpawn();

        int roomPick = Random.Range(0, rooms.Count);

        GameObject roomObj = InstantiateRoom(roomPick);
        RoomDescriber describer = roomObj.GetComponent<RoomDescriber>();

        roomSpawn.type = roomPick;
        roomSpawn.roomSize = describer.size;
        roomSpawn.location = new Vector2(x, y);
        roomSpawn.obj = roomObj;

        //add each doorway
        List<DoorDescriber> doors = new List<DoorDescriber>();
        for (int i = 0; i < describer.doorways.Count; i++)
        {
            doors.Add(describer.doorways[i].GetComponent<DoorDescriber>());
            doors[i].location = new Vector2(
                describer.doorways[i].transform.position.x, 
                describer.doorways[i].transform.position.y);
            doors[i].doorOpen = true;
        }
        roomSpawn.doors = doors;
        return roomSpawn;
    }

    private GameObject InstantiateRoom(int roomPick)
    {
        return Instantiate(rooms[roomPick], mapTransform);
    }
}
