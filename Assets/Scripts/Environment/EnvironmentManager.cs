using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Range(10, 1000)]
    public int maxElements;

    public GameObject environmentFire;
    public GameObject environmentWater;
    public GameObject environmentSteam;
    public GameObject environmentElectric;
    public GameObject environmentElectricWater;

    private Dictionary<string, GameObject> environmentDict;

    public GameObject player;

    Helper helper;

    // Start is called before the first frame update
    void Start()
    {
        environmentDict = new Dictionary<string, GameObject>();

        helper = new Helper();

        environmentDict.Add(Constants.TAG_ENVIRONMENT_FIRE, environmentFire);
        environmentDict.Add(Constants.TAG_ENVIRONMENT_WATER, environmentWater);
        environmentDict.Add(Constants.TAG_ENVIRONMENT_STEAM, environmentSteam);

        //environmentElements = new List<GameObject>();
    }

    IEnumerator RemoveEnvironments()
    {
        yield return new WaitForSeconds(0.2f);

        Transform environments = this.gameObject.transform;

        int numToDelete = environments.childCount - maxElements;

        if (numToDelete > 0)
        { 
            for (int j = 0; j < numToDelete; j++)
            {
                float farDistance = helper.getDistance(environments.GetChild(0).transform.position, player.transform.position);
                int farIndex = 0;

                for (int i = 1; i < environments.childCount; i++)
                {
                    float distance = helper.getDistance(environments.GetChild(i).transform.position, player.transform.position);
                    if (distance > farDistance)
                    {
                        farIndex = i;
                        farDistance = distance;
                    }
                }

                Destroy(environments.GetChild(farIndex).gameObject);
            }
        }

        yield return null;
    }

    public void AddEnvironmentFromTag(string tag, Vector2 position, float radius)
    {
        GameObject obj = Instantiate(environmentDict[tag], position, Quaternion.identity, this.gameObject.transform);
        obj.transform.localScale = new Vector2(radius, radius);


        StartCoroutine(RemoveEnvironments());
    }

    public void AddFire(Vector2 position, float radius)
    {
        AddEnvironmentFromTag(Constants.TAG_ENVIRONMENT_FIRE, position, radius);
    }

    public void AddWater(Vector2 position, float radius)
    {
        AddEnvironmentFromTag(Constants.TAG_ENVIRONMENT_WATER, position, radius);
    }

    public void AddSteam(Vector2 position, float radius)
    {
        AddEnvironmentFromTag(Constants.TAG_ENVIRONMENT_STEAM, position, radius);
    }

    public bool CheckForEnvironmentTag(string tag)
    {
        return (tag.Equals(Constants.TAG_ENVIRONMENT_FIRE) ||
            tag.Equals(Constants.TAG_ENVIRONMENT_STEAM) ||
            tag.Equals(Constants.TAG_ENVIRONMENT_WATER));
    }

    public void UpdateEnvironmentElements(GameObject element1, GameObject element2)
    {
        if (element1 == null || element2 == null) { return; } //This should only trigger if a race condition fails (I.E. element1 already called and this is element2 calling now)

        //check for same element
        if (element1.tag.Equals(element2.tag) && (element1.GetInstanceID() < element2.GetInstanceID())) //using InstanceID to decide which object will actual combine these two, as both objs will be calling this function
        {
           // CombineElements(element1, element2);
        }

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

    private bool ElementInUse(GameObject element)
    {
        return element.GetComponent<EnvironmentElement>().inUse;
    }
   /* private void CombineElements(GameObject e1, GameObject e2)
    {
        float radius = (e1.transform.localScale.x + e2.transform.localScale.x) * 0.7f;
        Vector2 position = Vector2.Lerp(e1.transform.position, e2.transform.position, 0.5f);
        string tag = e1.tag;

        Destroy(e2);
        Destroy(e1);

        Debug.Log($"combine! position:{position}, radius: {radius}, type: {tag}");
        AddEnvironmentFromTag(tag, position, radius);
    }*/
}
