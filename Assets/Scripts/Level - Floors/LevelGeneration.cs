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
        public List<Vector2> doorsLoc;
        public List<DoorDescriber> doorsDesc;
        public List<int> connections;
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

        Debug.Log("Instatiating rooms...");
        InstantiateRooms(roomSpawns);


        Debug.Log("Finished!");
    }

    //generates the locations to spawn preset rooms
    private List<RoomSpawn> SpawnRooms(int numRooms)
    {
        List<RoomSpawn> roomSpawns = new List<RoomSpawn>();

        //spawn first room
        roomSpawns.Add(GenerateRoom(0, 0));

        for (int i = 0; i < numRooms - 1; i++)
        {
            bool roomSpawnSuccess = true;
            int loopCount = 0;
            RoomSpawn roomSpawn;
            do
            {
                //pick room that already exists
                int roomPick = Random.Range(0, roomSpawns.Count);
                int doorPick = Random.Range(0, roomSpawns[roomPick].doorsLoc.Count);

                Vector2 doorPositionAdjust = GetDoorAdjust(roomSpawns[roomPick], doorPick);

                //find door of new room
                roomSpawn = GenerateRoom((int)roomSpawns[roomPick].location.x, (int)roomSpawns[roomPick].location.y);
                Vector2 roomMove = Vector2.zero;
                for (int j = 0; j < roomSpawn.doorsLoc.Count; j++)
                {
                    Vector2 doorAdjust = GetDoorAdjust(roomSpawn, j);
                    if (IsDoorInverse(doorPositionAdjust, doorAdjust))
                    {
                        Vector2 doorLoc = new Vector2(
                            roomSpawn.location.x + roomSpawn.doorsLoc[j].x,
                            roomSpawn.location.y + roomSpawn.doorsLoc[j].y);
                        Vector2 matchLoc = new Vector2(
                            roomSpawns[roomPick].location.x + roomSpawns[roomPick].doorsLoc[doorPick].x + doorPositionAdjust.x,
                            roomSpawns[roomPick].location.y + roomSpawns[roomPick].doorsLoc[doorPick].y + doorPositionAdjust.y);
                        Debug.Log($"matchLoc: {matchLoc}, doorLoc: {doorLoc}");
                        roomMove = matchLoc - doorLoc;

                        //remove used doors
                        roomSpawns[roomPick].doorsLoc.RemoveAt(doorPick);
                        roomSpawns[roomPick].doorsDesc.RemoveAt(doorPick);

                        roomSpawn.doorsLoc.RemoveAt(j);
                        roomSpawn.doorsDesc.RemoveAt(j);

                        break;
                    }
                }

                roomSpawn.location += roomMove;
                //update door locations
                for (int j = 0; j < roomSpawn.doorsLoc.Count; j++)
                {
                    roomSpawn.doorsLoc[j] += roomMove;
                }

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
            }
        }

        return roomSpawns;
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
        DoorDescriber doorDescriber = room.doorsDesc[door];
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

        RoomDescriber describer = rooms[roomPick].GetComponent<RoomDescriber>();

        roomSpawn.type = roomPick;
        roomSpawn.roomSize = describer.size;
        roomSpawn.location = new Vector2(x, y);
        roomSpawn.connections = new List<int>();

        //add each doorway
        List<Vector2> doors = new List<Vector2>();
        List<DoorDescriber> doorsDesc = new List<DoorDescriber>();
        for (int i = 0; i < describer.doorways.Count; i++)
        {
            doors.Add(new Vector2(
                describer.doorways[i].transform.position.x + x, 
                describer.doorways[i].transform.position.y + y));
            doorsDesc.Add(describer.doorways[i].GetComponent<DoorDescriber>());
        }
        roomSpawn.doorsLoc = doors;
        roomSpawn.doorsDesc = doorsDesc;
        return roomSpawn;
    }

    private void InstantiateRooms(List<RoomSpawn> roomSpawns)
    {
        foreach (RoomSpawn roomSpawn in roomSpawns)
        {
            GameObject room = Instantiate(rooms[roomSpawn.type], mapTransform);
            room.transform.position = roomSpawn.location;

            for (int i = 0; i < roomSpawn.doorsLoc.Count; i++)
            {
                GameObject door = Instantiate(tester);
                door.transform.position = roomSpawn.doorsLoc[i];
            }
        }
        Debug.Log("FINISH");
    }
    /*
    private List<Hall> GenerateHalls(List<RoomSpawn> roomSpawns)
    {
        List<Hall> halls = new List<Hall>();
        for (int i = 0; i < roomSpawns.Count; i++)
        {
            int closestRoom = FindClosestRoom(roomSpawns, i);
            Debug.Log($"Found room {closestRoom} as closest room to {i}");
            int closestDoorStartRoom = FindClosestDoor(roomSpawns[i], roomSpawns[closestRoom]);
            int closestDoorCloseRoom = FindClosestDoor(roomSpawns[closestRoom], roomSpawns[i]);

            List<Hall> newHalls = CreateHalls(roomSpawns[i].doors[closestDoorStartRoom], roomSpawns[closestRoom].doors[closestDoorCloseRoom]);
            halls.AddRange(newHalls);

            roomSpawns[i].doors.RemoveAt(closestDoorStartRoom);
            roomSpawns[closestRoom].doors.RemoveAt(closestDoorCloseRoom);
            roomSpawns[i].connections.Add(closestRoom);
            roomSpawns[closestRoom].connections.Add(i);
        }

        return halls;
    }

    private List<Hall> CreateHalls(Vector2 start, Vector2 end)
    {
        List<Hall> halls = new List<Hall>();
        bool finish = false;

        Vector2 cur = start;

        do
        {

            if (cur.x != end.x)
            {
                if (cur.x < end.x)
                {
                    cur.x += 1;
                }
                else
                {
                    cur.x -= 1;
                }
            }
            else if (cur.y != end.y)
            {
                if (cur.y < end.y)
                {
                    cur.y += 1;
                }
                else
                {
                    cur.y -= 1;
                }
            }

            Hall newHall = new Hall();
            newHall.type = 0;
            newHall.location = cur;
            halls.Add(newHall);

            if (cur.Equals(end))
            {
                finish = true;
            }

        } while (!finish);

        return halls;
    }

    private bool IsConnected(List<int> connections, int roomNum)
    {
        for (int i = 0; i < connections.Count; i++)
        {
            if (connections[i] == roomNum)
            {
                return true;
            }
        }
        return false;
    }

    private int FindClosestRoom(List<RoomSpawn> roomSpawns, int roomNum)
    {
        //find nearest room
        int nearestRoom = -1;
        float closestDistance = -1;
        for (int j = 0; j < roomSpawns.Count; j++)
        {
            if ((roomSpawns[j].location - roomSpawns[roomNum].location).magnitude < closestDistance || nearestRoom == -1)
            {
                if (j != roomNum && !IsConnected(roomSpawns[roomNum].connections, roomNum))
                { 
                    nearestRoom = j;
                    closestDistance = (roomSpawns[j].location - roomSpawns[roomNum].location).magnitude;
                }
            }
        }
        return nearestRoom;
    }

    private int FindClosestDoor(RoomSpawn doorRoom, RoomSpawn toRoom)
    {
        int closest = -1;
        float closestDistance = -1;
        for (int i = 0; i < doorRoom.doors.Count; i++)
        {
            if ((doorRoom.doors[i] - toRoom.location).magnitude < closestDistance || closest == -1)
            {
                closest = i;
                closestDistance = (doorRoom.doors[i] - toRoom.location).magnitude;
            }
        }

        return closest;
    }


    private void InstantiateHalls(List<Hall> halls)
    {
        foreach (Hall hall in halls)
        {
            GameObject room = Instantiate(hallways[hall.type], mapTransform);
            room.transform.position = hall.location;
        }
        Debug.Log("FINISH");
    }
    */
}
