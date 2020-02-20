using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{

    public GameObject environmentFire;
    public GameObject environmentWater;
    public GameObject environmentSteam;

    //List<GameObject> environmentElements;

    // Start is called before the first frame update
    void Start()
    {
        //environmentElements = new List<GameObject>();
    }

    public void AddFire(Vector2 position, float radius)
    {
        GameObject fire = Instantiate(environmentFire, position, Quaternion.identity, this.gameObject.transform);
        fire.transform.localScale = new Vector2(radius, radius);
    }

    public void AddWater(Vector2 position, float radius)
    {
        GameObject water = Instantiate(environmentWater, position, Quaternion.identity, this.gameObject.transform);
        water.transform.localScale = new Vector2(radius, radius);
    }

    public void AddSteam(Vector2 position, float radius)
    {
        GameObject steam = Instantiate(environmentSteam, position, Quaternion.identity, this.gameObject.transform);
        steam.transform.localScale = new Vector2(radius, radius);
    }

    public bool CheckForEnvironmentTag(string tag)
    {
        return (tag.Equals(Constants.TAG_ENVIRONMENT_FIRE) ||
            tag.Equals(Constants.TAG_ENVIRONMENT_STEAM) ||
            tag.Equals(Constants.TAG_ENVIRONMENT_WATER));
    }

    public void UpdateEnvironmentElements(GameObject element1, GameObject element2)
    {
        if (element1 == null || element2 == null) { Debug.Log("but they null"); return; } //This should only trigger if a race condition fails (I.E. element1 already called and this is element2 calling now)
        //check for fire + water = steam
        if (element1.tag.Equals(Constants.TAG_ENVIRONMENT_FIRE) && element2.tag.Equals(Constants.TAG_ENVIRONMENT_WATER))
        {
            float scale = (element1.transform.localScale.x + element2.transform.localScale.x) * Constants.ENVIRONMENT_STEAM_CREATE_RADIUS_MULTIPLIER;
            Vector2 position = Vector2.Lerp(element1.transform.position, element2.transform.position, 0.5f);

            //destroy previous objs
            Destroy(element1);
            Destroy(element2);
            //create steam obj
            AddSteam(position, scale);
        }
    }
}
