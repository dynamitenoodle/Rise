using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderManager : MonoBehaviour
{

    public GameObject traderPrefab;

    private GameObject trader;

    private List<LevelGeneration.RoomSpawn> rooms = null;

    private Helper helper;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        helper = new Helper();
        player = GameObject.FindGameObjectWithTag(Constants.TAG_PLAYER);
    }

    public void SetupRoomData(List<LevelGeneration.RoomSpawn> rooms)
    {
        this.rooms = rooms;
    }

    public void GenerateShop()
    {
        if (rooms == null) { Debug.LogError("Rooms are not setup for TraderManager"); return; }

        List<LevelGeneration.RoomSpawn> validRooms = new List<LevelGeneration.RoomSpawn>();

        foreach (LevelGeneration.RoomSpawn roomSpawn in rooms)
        {
            if (helper.getDistance(player.transform.position, roomSpawn.location) < Constants.WAVEGEN_SAFE_ZONE)
            {
                validRooms.Add(roomSpawn);    
            }
        }

        LevelGeneration.RoomSpawn randomRoom = validRooms[Random.Range(0, validRooms.Count - 1)];

        trader = Instantiate(traderPrefab, randomRoom.location, Quaternion.identity, this.transform);

        //spawn in shop items

    }

    private void GenerateShopItems()
    {

    }

    public void RemoveShop()
    {

    }
}
