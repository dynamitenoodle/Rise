using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public bool isAbility;
    public string name;
    public GameObject obj;
    public Ability ability;
    public Modifier modifier;
    public string description;
    public string image;
    public float costMultiplier;
    public int cost;
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
    public string image;
    public float cost;
    public byte poolType;
}


public class ItemPoolManager : MonoBehaviour
{

    public static ItemPoolManager Instance { get; private set; }

    List<List<Item>> abilityPool;
    List<List<Item>> modifierPool;

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

        modifierPool = LoadJSONItems("ItemPool_Modifiers", false);
        //abilityPool = LoadJSONItems("ItemPool_Abilities");
    }

    private List<List<Item>> LoadJSONItems(string fileName, bool isAbilities)
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
            itemDescription.isAbility = isAbilities;
            if (isAbilities)
            {
                itemDescription.ability = (Ability)System.Activator.CreateInstance(System.Type.GetType(item.className));
                itemDescription.modifier = null;
            }
            else
            {
                itemDescription.modifier = (Modifier)System.Activator.CreateInstance(System.Type.GetType(item.className));
                itemDescription.ability = null;
            }
            itemDescription.name = item.name;
            itemDescription.obj = Resources.Load<GameObject>(item.asset);
            itemDescription.description = item.description;
            itemDescription.costMultiplier = item.cost;
            itemDescription.cost = (int)Random.Range(((item.poolType + 1) * 40) * item.cost, (((item.poolType + 1) * 40) * 4) * item.cost);
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

        int randomItem = Random.Range(0, modifierPool[itemTier].Count);
        randomItem = 2;
        Item item = modifierPool[itemTier][randomItem];
        modifierPool[itemTier].RemoveAt(randomItem);
        return item;
    }

    public Item GetModifierFromPool(int overrideSpawnType)
    {
        return null;
    }

}
