using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject player;
    public Transform enemyTransform;
    public List<GameObject> enemyPrefabs;
    List<ElevatorDescriber> elevators = null;

    [HideInInspector]
    public List<GameObject> enemyQueue = null;

    //enemy count vars
    public int maxEnemiesForWave;
    public int minEnemiesForWave;

    //spawn chance vars
    public float commonSpawnChance;
    public float uncommonSpawnChance;
    public float rareSpawnChance;
    public float specialSpawnChance;

    //max enemy spawns vars
    //maxEnemyGroupSpawn is the number of enemies that can be spawned in a given group spawn (how many are spawned in at once)
    public int maxEnemyGroupSpawn;
    public int maxEnemiesOut;

    private int enemiesOut;

    private List<GameObject> commonEnemyPrefabs;
    private List<GameObject> uncommonEnemyPrefabs;
    private List<GameObject> rareEnemyPrefabs;
    private List<GameObject> specialEnemyPrefabs;

    bool spawnWave;

    Helper helper = new Helper();

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(50, 0, 0, 0.5f);
        Gizmos.DrawWireSphere(player.transform.position, Constants.WAVEGEN_SPAWN_ZONE);
        Gizmos.color = new Color(0, 50, 0, 0.5f);
        Gizmos.DrawWireSphere(player.transform.position, Constants.WAVEGEN_SAFE_ZONE);
    }

    private void Start()
    {
        commonEnemyPrefabs = new List<GameObject>();
        uncommonEnemyPrefabs = new List<GameObject>();
        rareEnemyPrefabs = new List<GameObject>();
        specialEnemyPrefabs = new List<GameObject>();

        //sort enemies into appropriate lists
        foreach (GameObject enemy in enemyPrefabs)
        {
            switch (enemy.GetComponent<EnemyScript>().enemySpawnType)
            {
                case EnemyScript.EnemySpawnType.common:
                    commonEnemyPrefabs.Add(enemy);
                    break;
                case EnemyScript.EnemySpawnType.uncommon:
                    uncommonEnemyPrefabs.Add(enemy);
                    break;
                case EnemyScript.EnemySpawnType.rare:
                    rareEnemyPrefabs.Add(enemy);
                    break;
                case EnemyScript.EnemySpawnType.special:
                    specialEnemyPrefabs.Add(enemy);
                    break;
            }
        }

        enemyQueue = new List<GameObject>();
        spawnWave = true;
        StartCoroutine("SpawnWave");
    }

    public void SetupElevators(List<GameObject> elevatorObjs)
    {
        elevators = new List<ElevatorDescriber>();
        foreach (GameObject elevator in elevatorObjs)
        {
            ElevatorDescriber elevatorDesc = elevator.GetComponent<ElevatorDescriber>();
            elevators.Add(elevatorDesc);
        }
    }


    IEnumerator SpawnWave()
    {
        for(;;)
        {
            if (elevators != null)
            {
                if (enemyQueue.Count == 0 && spawnWave)
                {
                    enemiesOut = 0;
                    GenerateEnemyQueue();
                }
                else
                {
                    int spawnNum = maxEnemyGroupSpawn;

                    if (maxEnemyGroupSpawn + enemiesOut > maxEnemiesOut)
                    {
                        spawnNum = (maxEnemyGroupSpawn + enemiesOut) - maxEnemiesOut;
                    }
                    if (spawnNum > enemyQueue.Count)
                    {
                        spawnNum = enemyQueue.Count;
                    }

                    SpawnEnemyGroup(spawnNum);
                    yield return new WaitForSeconds(10f);
                }
            }
            yield return new WaitForSeconds(2f);
        }
    }

    private void SpawnEnemyGroup(int enemyCount)
    {
        List<ElevatorDescriber> usedElevators = new List<ElevatorDescriber>();
        Debug.Log($"Spawning enemy group | Count: {enemyCount}   -   enemyQueue count: {enemyQueue.Count}");
        for (int i = 0; i < enemyCount; i++)
        {
            ElevatorDescriber elevatorSpawn = GetElevatorSpawn(usedElevators);

            int ranSpawnPoint = Random.Range(0, elevatorSpawn.spawnPoints.Count - 1);

            Vector2 spawnPoint = elevatorSpawn.spawnPoints[ranSpawnPoint].position;

            GameObject enemySpawn = Instantiate(enemyQueue[0], enemyTransform);
            enemySpawn.transform.position = spawnPoint;
            enemyQueue.RemoveAt(0);
            enemiesOut++;

            usedElevators.Add(elevatorSpawn);
        }
    }

    private bool IsElevatorUsed(ElevatorDescriber elevator, List<ElevatorDescriber> usedElevators)
    {
        foreach (ElevatorDescriber usedElevator in usedElevators)
        {
            if (elevator == usedElevator)
            {
                return true;
            }
        }
        return false;
    }

    private ElevatorDescriber GetElevatorSpawn(List<ElevatorDescriber> usedElevators)
    {
        List<ElevatorDescriber> validElevators = new List<ElevatorDescriber>();
        List<ElevatorDescriber> secondaryElevators = new List<ElevatorDescriber>();

        foreach (ElevatorDescriber elevator in elevators)
        {
            if (!IsElevatorUsed(elevator, usedElevators))
            {
                float elevatorDistance = helper.getDistance(player.transform.position, elevator.transform.position);
                if (elevatorDistance > Constants.WAVEGEN_SAFE_ZONE)
                {
                    if (elevatorDistance < Constants.WAVEGEN_SPAWN_ZONE)
                    {
                        validElevators.Add(elevator);
                    }
                    else
                    {
                        secondaryElevators.Add(elevator);
                    }
                }
            }
        }

        if (validElevators.Count > 0)
        {
            int ranElevator = Random.Range(0, validElevators.Count - 1);
            return validElevators[ranElevator];
        }
        else if (secondaryElevators.Count > 0)
        {
            //get closest secondary elevator
            float closestDistance = helper.getDistance(player.transform.position, secondaryElevators[0].transform.position);
            int closestElevator = 0;
            for (int i = 1; i < secondaryElevators.Count; i++)
            {
                float distance = helper.getDistance(player.transform.position, secondaryElevators[i].transform.position);
                if (distance < closestDistance)
                {
                    closestElevator = i;
                    closestDistance = distance;
                }
            }

            return secondaryElevators[closestElevator];
        }
        else
        {
            Debug.LogError("Unable to find valid elevator - return Vector3.zero");
            return null;
        }
    }

    private void GenerateEnemyQueue()
    {
        Debug.Log("Generating Enemy Queue");
        int enemySpawns = Random.Range(minEnemiesForWave, maxEnemiesForWave);
        Debug.Log($"Spawning {enemySpawns} enemies for wave");
        enemyQueue = new List<GameObject>();

        for (int i = 0; i < enemySpawns; i++)
        {
            float enemyTypeSpawn = Random.Range(0, 100) / 100.0f;
            GameObject enemy = null;

            if (enemyTypeSpawn <= specialSpawnChance)
                enemy = GetRandomEnemy(commonEnemyPrefabs);//TODO: change this to specialEnemyPrefabs
            else if (enemyTypeSpawn <= rareSpawnChance) 
                enemy = GetRandomEnemy(commonEnemyPrefabs);//TODO: change this to rareEnemyPrefabs
            else if (enemyTypeSpawn <= uncommonSpawnChance)
                enemy = GetRandomEnemy(uncommonEnemyPrefabs);
            else if (enemyTypeSpawn <= commonSpawnChance)
                enemy = GetRandomEnemy(commonEnemyPrefabs);
            else
                Debug.LogError("Enemy spawn chance went over 1.0?");

            if (enemy != null)
            {
                enemyQueue.Add(enemy);
            }
            else
            {
                Debug.LogError("Unable to add enemy as it is NULL");
            }
        }

    }

    private GameObject GetRandomEnemy(List<GameObject> enemyList)
    {
        return enemyList[Random.Range(0, enemyList.Count - 1)];
    }
}
