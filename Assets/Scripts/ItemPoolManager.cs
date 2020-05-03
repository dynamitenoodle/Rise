using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string name;
    public GameObject obj;
    public Ability ability;
    public Modifier modifier;
    public ItemPickup item;
    public string description;
    public float costMultiplier;
    public int cost;
    public int tier;
}

[System.Serializable]
public class AbilityList
{
    public List<Abilities> Abilities;
}

[System.Serializable]
public class Abilities
{
    public string name;
    public string asset;
    public string className;
    public string description;
    public float cost;
    public byte poolType;
}


public class ItemPoolManager : MonoBehaviour
{

    private enum LoadType
    {
        ability,
        modifier,
        item
    }


    public static ItemPoolManager Instance { get; private set; }

    List<List<Item>> abilityPool;
    List<List<Item>> modifierPool;
    List<List<Item>> itemPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    float[] spawnChances;

    // Start is called before the first frame update
    void Start()
    {
        spawnChances = new float[4];
        spawnChances[0] = Constants.ITEM_SPAWNCHANCE_COMMON;
        spawnChances[1] = Constants.ITEM_SPAWNCHANCE_UNCOMMON;
        spawnChances[2] = Constants.ITEM_SPAWNCHANCE_RARE;
        spawnChances[3] = Constants.ITEM_SPAWNCHANCE_SPECIAL;

        modifierPool = LoadJSONItems("ItemPool_Modifiers", LoadType.modifier);
        itemPool = LoadJSONItems("ItemPool_ItemPickups", LoadType.item);
        //abilityPool = LoadJSONItems("ItemPool_Abilities");
    }

    private List<List<Item>> LoadJSONItems(string fileName, LoadType loadType)
    {
        string json = GetJSONFromFile(fileName);

        //load abilities
        AbilityList jsonAbilitiesLoad = JsonUtility.FromJson<AbilityList>(json);

        List<List<Item>> itemsLoad = new List<List<Item>>();

        //add 4 lists for common, uncommon, rare, special
        itemsLoad.Add(new List<Item>()); //common
        itemsLoad.Add(new List<Item>()); //uncommon
        itemsLoad.Add(new List<Item>()); //rare
        itemsLoad.Add(new List<Item>()); //special

        foreach (Abilities item in jsonAbilitiesLoad.Abilities)
        {
            Item itemDescription = new Item();

            itemDescription.ability = null;
            itemDescription.modifier = null;
            itemDescription.item = null;
            itemDescription.cost = 0;

            if (loadType == LoadType.ability)
            {
                itemDescription.ability = (Ability)System.Activator.CreateInstance(System.Type.GetType(item.className));
            }
            else if (loadType == LoadType.modifier)
            {
                itemDescription.modifier = (Modifier)System.Activator.CreateInstance(System.Type.GetType(item.className));
                itemDescription.cost = (int)Random.Range(((item.poolType + 1) * 40) * item.cost, (((item.poolType + 1) * 40) * 4) * item.cost);
            }
            else if (loadType == LoadType.item)
            {
                itemDescription.item = (ItemPickup)System.Activator.CreateInstance(System.Type.GetType(item.className));
                itemDescription.cost = (int)Random.Range((30 * item.cost), (70 * item.cost));
            }

            itemDescription.name = item.name;
            itemDescription.obj = Resources.Load<GameObject>(item.asset);
            if (itemDescription.obj == null)
            {
                Debug.LogError("Failed to load object for item name: " + item.name);
            }
            itemDescription.description = item.description;
            itemDescription.costMultiplier = item.cost;
            itemDescription.tier = item.poolType;
            itemsLoad[item.poolType].Add(itemDescription);
        }

        return itemsLoad;
    }

    private string GetJSONFromFile(string filePath)
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>(filePath);
        return jsonAsset.text;
    }

    public Item GetModifierFromPool()
    {
        float randomItemTeir = Random.Range(0.0f, 1.0f);
        int itemTier = -1;
        for (int i = 0; i < 4; i++)
        {
            if (randomItemTeir < spawnChances[i])
            {
                itemTier = i;
                break;
            }
        }

        itemTier = 0;

        bool fixTier = true;
        do
        {
            if (modifierPool[itemTier].Count == 0 && itemTier != 3)
            {
                itemTier++;
            }
            else if (itemTier == 3)
            {
                return null;
            }
            else
            {
                fixTier = false;
            }
        } while (fixTier);

        int randomItem = Random.Range(0, modifierPool[itemTier].Count);

        Item item = modifierPool[itemTier][randomItem];
        modifierPool[itemTier].RemoveAt(randomItem);
        return item;
    }

    public Item GetItemPickupFromPool()
    {
        int randomItem = Random.Range(0, itemPool[0].Count);
        Item item = itemPool[0][randomItem];
        return item;
    }

    public Item GetItemForTrader()
    {
        Item item;
        float randomPickup = Random.Range(0.0f, 1.0f);

        if (randomPickup > 0.2f)
        {
            item = GetModifierFromPool();
            if (item == null)
            {
                item = GetItemPickupFromPool();
            }
        }
        else
        {
            item = GetItemPickupFromPool();
        }

        return item;
    }

    public void AddModifierToPoll(Item item)
    {
        modifierPool[item.tier].Add(item);
    }

    public Item GetModifierFromPool(int overrideSpawnType)
    {
        return null;
    }

}
