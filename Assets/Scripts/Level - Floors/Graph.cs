using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [HideInInspector] public List<Node> nodes;
    GameObject player;
    float nodeDis = 6f;
    float doorDis = 3f;

    // Start is called before the first frame update
    void Start()
    {
        nodes = new List<Node>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Setup the graph
    public void SetGraph()
    {
        // set the door nodes
        foreach(Node node in nodes)
        {
            if (!node.doorNode)
            {
                foreach (Node otherNode in nodes)
                {
                    if (!otherNode.doorNode && otherNode.roomNum != node.roomNum && Vector3.Distance(otherNode.pos, node.pos) <= doorDis)
                    {
                        otherNode.nearby.Add(node);
                        node.nearby.Add(otherNode);
                        otherNode.doorNode = true;
                        node.doorNode = true;
                        break;
                    }
                }
            }
        }

        player.GetComponent<PlayerScript>().RoomNum = FirstRoomCheck(player);
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
    Node GetNextNode(Vector3 enemyPosition)
    {
        Node nextNode;

        // figure out which node the player is closest to
        Node playerNode = nodes[0];
        foreach (Node node in nodes)
        {
            if (Vector3.Distance(playerNode.pos, player.transform.position) < Vector3.Distance(node.pos, player.transform.position))
            {
                playerNode = node;
                player.GetComponent<PlayerScript>().SetRoom(playerNode.roomNum);
            }
        }


        LowestCostSetup(playerNode);
        HeuristicSetup(playerNode);

        return null;
    }

    // Calculate lowest cost
    void LowestCostSetup(Node playerNode)
    {
        foreach (Node node in nodes)
        {

        }
    }

    // Calculate Heuristic
    void HeuristicSetup(Node playerNode)
    {
        foreach (Node node in nodes)
        {

        }
    }

    public int FirstRoomCheck(GameObject entity)
    {
        Node nearestNode = nodes[0];

        foreach (Node node in nodes)
        {
            if (node != nearestNode)
            {
                if (Vector3.Distance(nearestNode.pos, entity.transform.position) < Vector3.Distance(node.pos, entity.transform.position))
                {
                    nearestNode = node;
                }
            }
        }

        return nearestNode.roomNum;
    }
}
