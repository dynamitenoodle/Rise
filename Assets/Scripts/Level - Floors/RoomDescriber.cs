using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDescriber : MonoBehaviour
{
    public List<GameObject> doorways;
 
    public List<Vector4> roomSize;

    public List<Transform> enemyPathPoints;

    private void OnDrawGizmos()
    {
        foreach (Vector4 size in roomSize)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(new Vector3(size.x, size.y, 0) + gameObject.transform.position, new Vector3(size.z, size.w, 0));
        }
        foreach (Transform pos in enemyPathPoints)
        {
            Gizmos.color = new Color(50, 50, 50, 0.5f);
            Gizmos.DrawSphere(pos.position, 0.25f);
        }
    }
}
