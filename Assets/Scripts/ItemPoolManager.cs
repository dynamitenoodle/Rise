using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDescription
{
    public string name;
    public GameObject obj;
    public string description;
    public int costMultiplier;
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
}


public class ItemPoolManager : MonoBehaviour
{

    

    List<List<ItemDescription>> abilityPool;
    List<List<ItemDescription>> modifierPool;


    // Start is called before the first frame update
    void Start()
    {
        string json = GetJSONFromFile("ItemPool_Abilities");
        Debug.Log(json);
        //load abilities
        AbilityList jsonAbilitiesLoad = JsonUtility.FromJson<AbilityList>(json);

        foreach (Abilities value in jsonAbilitiesLoad.Abilities)
        {
            Debug.Log(value.name);
        }
    }

    private string GetJSONFromFile(string filePath)
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>(filePath);

        return jsonAsset.text;
    }
}
