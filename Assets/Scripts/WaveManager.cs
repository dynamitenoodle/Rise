using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    List<ElevatorDescriber> elevators = null;

    public List<GameObject> enemies;

    public void SetupElevators(List<GameObject> elevatorObjs)
    {
        foreach (GameObject elevator in elevatorObjs)
        {
            ElevatorDescriber elevatorDesc = elevator.GetComponent<ElevatorDescriber>();
            elevators.Add(elevatorDesc);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (elevators != null)
        {
            //define some way to know when the wave should start here
            SpawnWave();
        }
    }

    private void SpawnWave()
    {

    }
}
