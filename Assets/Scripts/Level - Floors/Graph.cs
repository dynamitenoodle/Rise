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

        player.GetComponent<PlayerScript>().SetNode(FirstRoomCheck(player.transform.position));
        Debug.Log("Player: " + player.GetComponent<PlayerScript>().Node.roomNum);

        player.GetComponent<PlayerScript>().Node.heuristic = 0;
        player.GetComponent<PlayerScript>().Node.isEnd = true;
        Heuristic(player.GetComponent<PlayerScript>().Node);
        player.GetComponent<PlayerScript>().Node.isEnd = false;
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
    public Node GetNextNode(Node node)
    {
        Node nextNode;

        // figure out which node the player is closest to
        player.GetComponent<PlayerScript>().SetNode(FirstRoomCheck(player.transform.position));
        Node playerNode = player.GetComponent<PlayerScript>().Node;

        node.lowestCost = 0;
        node.isStart = true;
        //LowestCost(playerNode);
        node.isStart = false;

        nextNode = node.nearby[0];

        foreach (Node n in node.nearby)
        {
            if (n.heuristic < nextNode.heuristic)
                nextNode = n;
        }

        return nextNode;
    }

    // Calculate lowest cost
    void LowestCost(Node enemyNode)
    {
        foreach (Node node in nodes)
        {

        }
    }

    // Calculate Heuristic
    void Heuristic(Node currentNode)
    {
        foreach (Node node in currentNode.nearby)
        {
            float dis = Vector3.Distance(node.pos, currentNode.pos);
            float newHeuristic = dis + currentNode.heuristic;
            if (!node.isEnd && (node.heuristic == 0 || node.heuristic > newHeuristic))
            {
                node.heuristic = newHeuristic;
                Heuristic(node);
            }
        }
        
    }

    public Node FirstRoomCheck(Vector3 position)
    {
        Node nearestNode = nodes[0];

        foreach (Node node in nodes)
        {
            if (node != nearestNode)
            {
                if (Vector3.Distance(node.pos, position) < Vector3.Distance(nearestNode.pos, position))
                {
                    nearestNode = node;
                }
            }
        }

        return nearestNode;
    }
}
