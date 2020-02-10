using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDescriber : MonoBehaviour
{
    public List<Transform> spawnPoints;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Transform point in spawnPoints)
            Gizmos.DrawSphere(point.position, 0.5f);
    }
}
