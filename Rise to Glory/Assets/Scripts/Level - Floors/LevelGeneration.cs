using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    //public vars for adjustments
    public int mapSize;
    public int minRoomSpawns;
    public int maxRoomSpawns;

    public int maxMoveLength;
    public int minMoveLength;

    public List<GameObject> rooms;

    //class vars
    Helper helper;

    struct RoomSpawn
    {
        public int type;
        public Vector2 location;
        public int roomSize;                                
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
        int numRooms = Random.Range(minRoomSpawns, maxRoomSpawns);
        List<RoomSpawn> roomSpawns = SpawnRooms(numRooms);


    }

    //generates the locations to spawn preset rooms
    private List<RoomSpawn> SpawnRooms(int numRooms)
    {
        List<RoomSpawn> roomSpawns = new List<RoomSpawn>();

        //spawn first room
        roomSpawns.Add(GenerateRoom(0, 0));

        for (int i = 0; i <= numRooms; i++)
        {
            bool roomSpawnSuccess = true;
            int loopCount = 0;
            RoomSpawn roomSpawn;
            do
            {
                //pick room that already exists
                int existingRoom = Random.Range(0, roomSpawns.Count);

                //get x move
                int moveX = Random.Range(-maxMoveLength, maxMoveLength);
                if (moveX < 0) { moveX -= minMoveLength; }
                else { moveX += minMoveLength; }
                //get y move
                int moveY = Random.Range(-maxMoveLength, maxMoveLength);
                if (moveY < 0) { moveY -= minMoveLength; }
                else { moveY += minMoveLength; }

                //generate room
                Vector2 existingLoc = roomSpawns[existingRoom].location;
                roomSpawn = GenerateRoom((int)existingLoc.x + moveX, (int)existingLoc.y + moveY);

                for (int j = 0; j < roomSpawns.Count; j++)
                {

                    if ((roomSpawn.location - roomSpawns[j].location).magnitude < helper.getLarger(roomSpawn.roomSize, roomSpawns[j].roomSize) + Constants.LEVELGENERATION_MIN_ROOM_DISTANCE)
                    {
                        roomSpawnSuccess = false;
                        roomSpawn = new RoomSpawn();
                        roomSpawn.type = -1;
                    }
                }

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

    //generates a random room with a given location to pass back to room spawning method
    private RoomSpawn GenerateRoom(int x, int y)
    {
        RoomSpawn roomSpawn = new RoomSpawn();

        int roomPick = Random.Range(0, rooms.Count);

        RoomDescriber describer = rooms[roomPick].GetComponent<RoomDescriber>();

        roomSpawn.type = roomPick;
        roomSpawn.roomSize = describer.size;
        roomSpawn.location = new Vector2(x, y);

        return roomSpawn;
    }

}
