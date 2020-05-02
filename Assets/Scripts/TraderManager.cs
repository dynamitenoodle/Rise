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

    private MinimapController minimapController;

    List<Item> items;
    List<GameObject> itemObjs;

    // Start is called before the first frame update
    void Start()
    {
        helper = new Helper();
        player = GameObject.FindGameObjectWithTag(Constants.TAG_PLAYER);
        playerScript = player.GetComponent<PlayerScript>();

        minimapController = GameObject.Find(Constants.GAMEOBJECT_NAME_MINIMAPCAMERA).GetComponent<MinimapController>();

        levelManagerTransform = GameObject.Find(Constants.GAMEOBJECT_NAME_LEVELMANAGER).transform;
        itemUI = GameObject.Find(Constants.GAMEOBJECT_NAME_CANVAS).GetComponent<ItemDescriptionUI>();
        items = new List<Item>();
    }

    private void Update()
    {
        bool active = false;
        for (int i = 0; i < items.Count; i++)
        {
            if (helper.getDistance(player.transform.position, itemObjs[i].transform.position) < Constants.TRADER_ITEMUI_ACTIVE_DISTANCE)
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
                    Destroy(itemObjs[i]);
                    itemObjs.RemoveAt(i);
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

        GenerateShopItems();
        minimapController.ShowTraderArrow(trader);
    }

    private void GenerateShopItems()
    {
        //spawn in shop items
        items = new List<Item>();
        float offset = 1.0f;
        float initialOffset = 0.5f;
        itemObjs = new List<GameObject>();
        for (int i = 0; i < Constants.TRADER_ITEM_SPAWN_COUNT; i++)
        {
            Item item = ItemPoolManager.Instance.GetModifierFromPool();

            itemObjs.Add(Instantiate(item.obj, trader.transform));
            Vector3 pos = itemObjs[i].transform.position;
            pos.x += -initialOffset + (offset * i);
            itemObjs[i].transform.position = pos;
            items.Add(item);
        }
    }

    public void RemoveShop()
    {
        if (trader == null) { return; }

        Debug.Log("removing shop");

        if (items.Count > 0)
        {
            foreach (Item item in items)
            {
                ItemPoolManager.Instance.AddModifierToPoll(item);
            }
        }

        Destroy(trader);
        trader = null;
        items = new List<Item>();

        minimapController.HideTraderArrow();
    }
}
