using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [HideInInspector] public List<Node> nodes;
    float nodeDis = 5f;
    float doorDis = 3f;

    // Start is called before the first frame update
    void Start()
    {
        nodes = new List<Node>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Setup the graph
    public void SetGraph()
    {
        // set the positions
        
    }

    public void AddNodes(List<Transform> listOfPoints, int rmNum)
    {
        List<Node> tempNodes = new List<Node>();

        foreach (Transform t in listOfPoints)
        {
            t.gameObject.AddComponent<Node>();
            Node node = t.gameObject.GetComponent<Node>();

            node.pos = t.position;
            node.roomNum = rmNum;

            foreach (Node otherNode in tempNodes)
            {
                if (Vector3.Distance(otherNode.pos, node.pos) < nodeDis)
                {
                    otherNode.nearby.Add(node);
                    node.nearby.Add(otherNode);
                }
            }

            tempNodes.Add(node);
        }

        nodes.AddRange(tempNodes);
    }
}
