using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{

    public GameObject environmentFire;

    List<GameObject> environmentElements;

    // Start is called before the first frame update
    void Start()
    {
        environmentElements = new List<GameObject>();
    }

    public void AddFire(Vector2 position, float radius)
    {
        //Debug.Log("Add fire to position: " + position);
        GameObject fire = Instantiate(environmentFire, position, Quaternion.identity, this.gameObject.transform);
        fire.transform.localScale = new Vector2(radius, radius);

        environmentElements.Add(fire);
    }

}
