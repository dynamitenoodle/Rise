using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // attributes
    public Vector3 pos;
    public float heur;
    public float low;
    /* Don't need these? Only ever checking it being updated to find the player? ayayayayayaya
    bool visited;
    bool playerNode;
    bool current;
    bool previous;
    */
    public int roomNum;
    public List<Node> nearby;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}