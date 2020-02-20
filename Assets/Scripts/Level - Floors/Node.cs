using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // attributes
    public Vector3 pos;
    public float heuristic;
    public float lowestCost;
    /* Don't need these? Only ever checking it being updated to find the player? ayayayayayaya
    bool visited;
    bool playerNode;
    bool current;
    bool previous;
    */
    public bool isStart;
    public bool isEnd;
    public bool doorNode;
    public int roomNum;
    public List<Node> nearby;

}