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
    private PlayerScript playerScript;
    private Transform levelManagerTransform;

    private ItemDescriptionUI itemUI;

    List<Item> items;

    // Start is called before the first frame update
    void Start()
    {
        helper = new Helper();
        player = GameObject.FindGameObjectWithTag(Constants.TAG_PLAYER);
        playerScript = player.GetComponent<PlayerScript>();
        levelManagerTransform = GameObject.Find(Constants.GAMEOBJECT_NAME_LEVELMANAGER).transform;
        itemUI = GameObject.Find(Constants.GAMEOBJECT_NAME_CANVAS).GetComponent<ItemDescriptionUI>();
        items = new List<Item>();
    }

    private void Update()
    {
        bool active = false;
        for (int i = 0; i < items.Count; i++)
        {
            if (helper.getDistance(player.transform.position, items[i].obj.transform.position) < Constants.TRADER_ITEMUI_ACTIVE_DISTANCE)
            {
                active = true;
                itemUI.SetUIData(items[i]);
                itemUI.ShowUI(true);

                bool canBuy = playerScript.HasGold(items[i].cost);

                itemUI.SetCostCanBuy(canBuy);

                if (Input.GetKeyDown(KeyCode.E) && canBuy)
                {
                    playerScript.AddModifier(items[i]);
                    playerScript.AddGold(-items[i].cost);
                    Destroy(items[i].obj);
                    items.RemoveAt(i);
                }
            }
        }
        if (!active)
        {
            itemUI.ShowUI(false);
        }
    }

    public void SetupRoomData(List<LevelGeneration.RoomSpawn> rooms)
    {
        this.rooms = rooms;
    }

    public void GenerateShop()
    {
        Debug.Log("Generating shop");
        if (rooms == null) { Debug.LogError("Rooms are not setup for TraderManager"); return; }

        List<LevelGeneration.RoomSpawn> validRooms = new List<LevelGeneration.RoomSpawn>();

        foreach (LevelGeneration.RoomSpawn roomSpawn in rooms)
        {
            if (helper.getDistance(player.transform.position, roomSpawn.location) > Constants.WAVEGEN_SAFE_ZONE)
            {
                validRooms.Add(roomSpawn);    
            }
        }

        int roomPick = Random.Range(0, validRooms.Count - 1);
        //Debug.Log($"validRoom Count: {validRooms.Count} | roomPick: {roomPick}");
        LevelGeneration.RoomSpawn randomRoom = validRooms[roomPick];

        trader = Instantiate(traderPrefab, randomRoom.location, Quaternion.identity, levelManagerTransform);

        //spawn in shop items
        items = new List<Item>();
        for (int i = 0; i < 1; i ++)
        {
            Item item = ItemPoolManager.Instance.GetModifierFromPool();

            GameObject itemObj = Instantiate(item.obj, trader.transform);
            item.obj = itemObj;
            items.Add(item);
        }
    }

    private void GenerateShopItems()
    {

    }

    public void RemoveShop()
    {
        Debug.Log("removing shop");
        Destroy(trader);
        items = new List<Item>();
    }
}
