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
        //type of room (based on given available rooms {rooms})
        public int type;
        //location of room
        public Vector2 location;
        //representation of dimensions of the room (using x, y, width, height for each Vector4)
        public List<Vector4> roomSize;
        //List of all doors attached to the room
        public List<DoorDescriber> doors;
        //GameObject of the room
        public GameObject obj;
    }

    // Start is called before the first frame update
    void Start()
    {
        helper = new Helper();
        GenerateLevel();
    }

    /// <summary>
    /// Main method to generate everything (basically start point for level generation)
    /// </summary>
    private void GenerateLevel()
    {
        //spawn rooms
        Debug.Log("Generating rooms...");
        int numRooms = Random.Range(minRoomSpawns, maxRoomSpawns);
        Debug.Log($"NumRooms: {numRooms}");
        List<RoomSpawn> roomSpawns = SpawnRooms(numRooms);

        //This for loop is just to instatiate the tester circles to visualize where the open doors are
        //this should be removed when done testing
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

    /// <summary>
    /// Generates a level based on a given number of rooms
    /// </summary>
    /// <param name="numRooms"></param>
    /// <returns></returns>
    private List<RoomSpawn> SpawnRooms(int numRooms)
    {
        List<RoomSpawn> roomSpawns = new List<RoomSpawn>();

        //spawn in random first room at (0,0)
        roomSpawns.Add(GenerateRoom(0, 0));
        roomSpawns[0].obj.transform.position = roomSpawns[0].location;

        //this var keeps track of how many times a single room has had to redo its process
        int retryCount = 0;

        //* numRooms - 1 because the first room was already spawned in 
        for (int i = 0; i < numRooms - 1; i++)
        {
            RoomSpawn roomSpawn; // this will be the variable for the room spawned in
            List<int> usedDoors = new List<int>();
            bool validRoom = true;
            bool forceFinish = false;
            
            //pick a random valid room that already exists and generate the room with location based off randomly picked room
            int roomPick = GetRandomValidRoom(roomSpawns);
            roomSpawn = GenerateRoom((int)roomSpawns[roomPick].location.x, (int)roomSpawns[roomPick].location.y);

            do
            {
                //pick a random open door from randomly picked room
                int doorPick = GetRandomValidDoor(roomSpawns[roomPick], usedDoors);

                //if no available doors, destroy generated room and restart process
                if (doorPick == -1)
                {
                    Destroy(roomSpawn.obj);
                    forceFinish = true;
                    validRoom = true;
                }
                else
                {
                    Vector2 doorPositionAdjust = GetDoorAdjust(roomSpawns[roomPick], doorPick);

                    //find appropriate door on spawned room to the randomly picked room
                    for (int j = 0; j < roomSpawn.doors.Count; j++)
                    {
                        Vector2 doorAdjust = GetDoorAdjust(roomSpawn, j);
                        if (IsDoorInverse(doorPositionAdjust, doorAdjust))
                        {
                            //move room to correct location based off picked door
                            Vector2 doorLoc = new Vector2(
                                roomSpawn.location.x + roomSpawn.doors[j].location.x,
                                roomSpawn.location.y + roomSpawn.doors[j].location.y);
                            Vector2 matchLoc = new Vector2(
                                roomSpawns[roomPick].location.x + roomSpawns[roomPick].doors[doorPick].location.x + doorPositionAdjust.x,
                                roomSpawns[roomPick].location.y + roomSpawns[roomPick].doors[doorPick].location.y + doorPositionAdjust.y);

                            roomSpawn.location += (matchLoc - doorLoc);
                            roomSpawn.obj.transform.position = roomSpawn.location;

                            //check for any intersecting rooms (try a different door if this room intersects with something)
                            bool intersecting = CheckRoomIntersections(roomSpawns, roomSpawn);
                            if (intersecting)
                            {
                                usedDoors.Add(doorPick);
                                validRoom = false;
                            }
                            else
                            {
                                //valid room! remove used doors
                                roomSpawns[roomPick].doors[doorPick].doorOpen = false;
                                roomSpawn.doors[j].doorOpen = false;
                                validRoom = true;
                            }
                            break;
                        }
                    }
                }
            } while (!validRoom);

            if (validRoom && !forceFinish)
            {
                //add room to list of existing rooms and update doors if any doors are now connected to existing rooms
                roomSpawns.Add(roomSpawn);
                CheckOverlapDoors(roomSpawns);
                retryCount = 0;
            }
            else
            {
                i--;
                retryCount++;
                Debug.Log($"Retrying room - unable to place room down anywhere | retry count: {retryCount}");
                if (retryCount > 10)
                {
                    Debug.LogWarning($"Retry count getting high - if this continues we may need to make a change to prevent further loops | retry Count: {retryCount}");
                }
            }
        }

        //return full list of generated rooms
        return roomSpawns;
    }

    /// <summary>
    /// Check to see if a given room {roomCheck} is colliding with any existing rooms {roomSpawns}
    /// </summary>
    /// <param name="roomSpawns"></param>
    /// <param name="roomCheck"></param>
    /// <returns></returns>
    private bool CheckRoomIntersections(List<RoomSpawn> roomSpawns, RoomSpawn roomCheck)
    {
        List<Vector4> sizesCheck = roomCheck.roomSize;

        foreach (RoomSpawn room in roomSpawns)
        {
            List<Vector4> sizes = room.roomSize;
            foreach (Vector4 sizeCheck in sizesCheck)
            {
                foreach (Vector4 size in sizes)
                {
                    bool check = helper.AABBTest(new Vector2(sizeCheck.x + roomCheck.location.x, sizeCheck.y + roomCheck.location.y), new Vector2(sizeCheck.z, sizeCheck.w),
                                                 new Vector2(size.x + room.location.x, size.y + room.location.y), new Vector2(size.z, size.w));
                    if (check)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Check existing rooms to see if any doors are overlapping, i.e two doors are next to each other that are open but shouldn't be
    /// </summary>
    /// <param name="roomSpawns"></param>
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

    /// <summary>
    /// Return the integer of a random OPEN door from a given room
    /// </summary>
    /// <param name="roomSpawn"></param>
    /// <param name="usedDoors"></param>
    /// <returns></returns>
    private int GetRandomValidDoor(RoomSpawn roomSpawn, List<int> usedDoors)
    {
        List<int> validDoors = new List<int>();

        for (int i = 0; i < roomSpawn.doors.Count; i++)
        {
            bool valid = true;
            for (int j = 0; j < usedDoors.Count; j++)
            {
                if (i == usedDoors[j])
                    valid = false;
            }

            if (valid && roomSpawn.doors[i].doorOpen)
            {
                validDoors.Add(i);
            }
        }

        if (validDoors.Count == 0)
        {
            return -1;
        }

        return validDoors[Random.Range(0, validDoors.Count - 1)];
    }

    /// <summary>
    /// Return a random VALID room from a given list (this means a room that has at least 1 valid/open door)
    /// </summary>
    /// <param name="roomSpawns"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Return true if the two doors are inverses of eachother (ie. one is north and one is south / one east and one west)
    /// </summary>
    /// <param name="door"></param>
    /// <param name="door2"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Find adjustment of door based on door's location (ex. north door should adjust up one space)
    /// </summary>
    /// <param name="room"></param>
    /// <param name="door"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Generate a random room based on {rooms} at a given position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private RoomSpawn GenerateRoom(int x, int y)
    {
        RoomSpawn roomSpawn = new RoomSpawn();

        int roomPick = Random.Range(0, rooms.Count);

        GameObject roomObj = InstantiateRoom(roomPick);
        RoomDescriber describer = roomObj.GetComponent<RoomDescriber>();

        roomSpawn.type = roomPick;
        roomSpawn.roomSize = describer.roomSize;
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

    /// <summary>
    /// Instatiate a given room type
    /// </summary>
    /// <param name="roomPick"></param>
    /// <returns></returns>
    private GameObject InstantiateRoom(int roomPick)
    {
        return Instantiate(rooms[roomPick], mapTransform);
    }
}
