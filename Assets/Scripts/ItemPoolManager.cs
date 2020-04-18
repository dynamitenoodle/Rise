using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string name;
    public GameObject obj;
    public string description;
    public float costMultiplier;
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
    public string description;
    public float cost;
    public byte poolType;
}


public class ItemPoolManager : MonoBehaviour
{

    public static ItemPoolManager Instance { get; private set; }

    List<List<Item>> abilityPool;
    List<List<Item>> modifierPool;


    float[] spawnChances;

    // Start is called before the first frame update
    void Start()
    {
        spawnChances = new float[4];
        spawnChances[0] = Constants.ITEM_SPAWNCHANCE_COMMON;
        spawnChances[1] = Constants.ITEM_SPAWNCHANCE_UNCOMMON;
        spawnChances[2] = Constants.ITEM_SPAWNCHANCE_RARE;
        spawnChances[3] = Constants.ITEM_SPAWNCHANCE_SPECIAL;

        modifierPool = LoadJSONItems("ItemPool_Modifiers");
        abilityPool = LoadJSONItems("ItemPool_Abilities");
    }

    private List<List<Item>> LoadJSONItems(string fileName)
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
            itemDescription.name = item.name;
            itemDescription.obj = Resources.Load<GameObject>(item.asset);
            itemDescription.description = item.description;
            itemDescription.costMultiplier = item.cost;
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

        int randomItem = Random.Range(0, modifierPool[itemTier].Count - 1);

        Item item = modifierPool[itemTier][randomItem];
        modifierPool[itemTier].RemoveAt(randomItem);
        return item;
    }

    public Item GetModifierFromPool(int overrideSpawnType)
    {
        return null;
    }

}
