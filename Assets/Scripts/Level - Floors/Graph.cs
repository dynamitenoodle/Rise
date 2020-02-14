using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [HideInInspector] public List<Node> nodes;
    float nodeDis = 6f;
    float doorDis = 3f;

    // Start is called before the first frame update
    void Start()
    {
        nodes = new List<Node>();
    }

    // Setup the graph
    public void SetGraph()
    {
        // set the door nodes
        foreach(Node node in nodes)
        {
            
        }

    }

    public void AddNodes(List<Transform> listOfPoints, int rmNum)
    {
        // Make a new list of nodes
        List<Node> tempNodes = new List<Node>();

        foreach (Transform t in listOfPoints)
        {
            // Let's make it a node
            t.gameObject.AddComponent<Node>();
            Node node = t.gameObject.GetComponent<Node>();

            node.pos = t.position;
            node.roomNum = rmNum;
            node.nearby = new List<Node>();

            // Let's see if there is any nodes nearby!
            foreach (Node otherNode in tempNodes)
            {
                if (Vector3.Distance(otherNode.pos, node.pos) <= nodeDis)
                {
                    otherNode.nearby.Add(node);
                    node.nearby.Add(otherNode);
                }
            }

            tempNodes.Add(node);
        }

        nodes.AddRange(tempNodes);
    }

    // Enemy calls this method
    Node GetNextNode()
    {

    }
}
