using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public GameObject player;
    public Transform enemyTransform;
    public List<GameObject> enemyPrefabs;
    List<ElevatorDescriber> elevators = null;
    GameObject bossRoom = null;
    GameObject boss = null;

    [HideInInspector]
    public List<GameObject> enemyQueue = null;
    [HideInInspector]
    public List<GameObject> enemies;

    //enemy count vars
    public int[] maxEnemiesForWave;
    public int[] minEnemiesForWave;

    public Gradient spawnChance;

    public Text enemyText;

    //max enemy spawns vars
    //maxEnemyGroupSpawn is the number of enemies that can be spawned in a given group spawn (how many are spawned in at once)
    public int maxEnemyGroupSpawn;
    public int maxEnemiesOut;

    private List<GameObject> commonEnemyPrefabs;
    private List<GameObject> uncommonEnemyPrefabs;
    private List<GameObject> rareEnemyPrefabs;
    private List<GameObject> specialEnemyPrefabs;

    bool spawnWave;

    Helper helper = new Helper();
    TraderManager traderManager;

    private int waveNum = 0;
    float waveTimer;
    float waveSpawnTimer;

    [Range(10, 120)]
    public float waveTimeToWait;

    private bool traderOut;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(50, 0, 0, 0.5f);
        Gizmos.DrawWireSphere(player.transform.position, Constants.WAVEGEN_SPAWN_ZONE);
        Gizmos.color = new Color(0, 50, 0, 0.5f);
        Gizmos.DrawWireSphere(player.transform.position, Constants.WAVEGEN_SAFE_ZONE);
    }

    private void Start()
    {
        enemies = new List<GameObject>();
        commonEnemyPrefabs = new List<GameObject>();
        uncommonEnemyPrefabs = new List<GameObject>();
        rareEnemyPrefabs = new List<GameObject>();
        specialEnemyPrefabs = new List<GameObject>();

        traderManager = GameObject.Find(Constants.GAMEOBJECT_NAME_LEVELMANAGER).GetComponent<TraderManager>();

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
        waveNum = -1;
        waveTimer = Time.time;
        waveSpawnTimer = Time.time;
        StartCoroutine(SpawnWave());
    }

    public void SetupElevators(List<GameObject> elevatorObjs, GameObject bossRoom, GameObject boss)
    {
        elevators = new List<ElevatorDescriber>();
        foreach (GameObject elevator in elevatorObjs)
        {
            ElevatorDescriber elevatorDesc = elevator.GetComponent<ElevatorDescriber>();
            elevators.Add(elevatorDesc);
        }

        this.bossRoom = bossRoom;
        this.boss = boss;
    }

    public void DestroyEnemy(GameObject enemy)
    {
        int index = -1;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].Equals(enemy))
            {
                index = i;
                break;
            }
        }

        if (index != -1)
        {
            enemies.RemoveAt(index);
        }
        else
        {
            Debug.LogError("Unable to find enemy to remove from enemies list?");
        }
    }

    IEnumerator SpawnWave()
    {
        bool spawning = true;
        while (spawning)
        {
            if (Time.time - waveTimer < waveTimeToWait && enemies.Count == 0)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    waveTimer -= waveTimeToWait;
                }
                 enemyText.text = $"Time to Wave {waveNum+1}: {Mathf.Round((waveTimeToWait - (Time.time - waveTimer)))}";
            }
            else if (elevators != null)
            {
                if (enemyQueue.Count == 0 && spawnWave)
                {
                    waveNum++;

                    if (waveNum == 5)
                    {
                        spawning = false;
                        yield return null;
                    }

                    waveTimer = Time.time;
                    if (waveNum == 4) //5th wave = boss wave
                        SpawnBoss();
                    else
                        GenerateEnemyQueue();

                    spawnWave = false;

                    traderManager.GenerateShop();
                    traderOut = true;
                }
                else if (enemyQueue.Count == 0 && !spawnWave && enemies.Count == 0)
                {
                    spawnWave = true;
                }
                else
                {

                    if (Time.time - waveSpawnTimer < Constants.WAVEGEN_GROUP_SPAWN_TIME) { }
                    else
                    {
                        if (traderOut)
                        {
                            traderManager.RemoveShop();
                        }

                        int spawnNum = maxEnemyGroupSpawn;

                        if (spawnNum + enemies.Count > maxEnemiesOut)
                        {
                            spawnNum = spawnNum - ((spawnNum + enemies.Count) - maxEnemiesOut);
                        }
                        if (spawnNum > enemyQueue.Count)
                        {
                            spawnNum = enemyQueue.Count;
                        }

                        SpawnEnemyGroup(spawnNum);

                        waveSpawnTimer = Time.time;

                        enemyText.text = $"Wave: {waveNum + 1}\nEnemies Remaining: {enemyQueue.Count + enemies.Count}\nEnemies Out: {enemies.Count}";
                    }

                    yield return new WaitForSeconds(1f);
                }
                enemyText.text = $"Wave: {waveNum+1}\nEnemies Remaining: {enemyQueue.Count + enemies.Count}\nEnemies Out: {enemies.Count}";
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
    }

    private void SpawnEnemyGroup(int enemyCount)
    {
        List<ElevatorDescriber> usedElevators = new List<ElevatorDescriber>();

        for (int i = 0; i < enemyCount; i++)
        {
            ElevatorDescriber elevatorSpawn = GetElevatorSpawn(usedElevators);

            int ranSpawnPoint = Random.Range(0, elevatorSpawn.spawnPoints.Count - 1);

            Vector2 spawnPoint = elevatorSpawn.spawnPoints[ranSpawnPoint].position;

            GameObject enemySpawn = Instantiate(enemyQueue[0], enemyTransform);
            enemySpawn.transform.position = spawnPoint;
            enemyQueue.RemoveAt(0);
            enemies.Add(enemySpawn);

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

    private void SpawnBoss()
    {
        ElevatorDescriber bossRoomDescriber = bossRoom.GetComponent<ElevatorDescriber>();

        player.transform.position = bossRoomDescriber.spawnPoints[0].position; //set player position to spawn point 0

        //spawn boss at spawn point 1
        GameObject bossObj = Instantiate(boss, bossRoomDescriber.spawnPoints[1].position, Quaternion.identity, enemyTransform);
        enemyQueue.Add(bossObj);
    }

    private void GenerateEnemyQueue()
    {
        //Debug.Log("Generating Enemy Queue");
        int enemySpawns = Random.Range(minEnemiesForWave[waveNum], maxEnemiesForWave[waveNum]);
        //Debug.Log($"Spawning {enemySpawns} enemies for wave");
        enemyQueue = new List<GameObject>();

        for (int i = 0; i < enemySpawns; i++)
        {
            float enemyTypeSpawn = Random.Range(0, 100) / 100.0f;
            GameObject enemy = null;

            if (enemyTypeSpawn <= spawnChance.colorKeys[0].time)
                enemy = GetRandomEnemy(commonEnemyPrefabs);//TODO: change this to specialEnemyPrefabs
            else if (enemyTypeSpawn <= spawnChance.colorKeys[1].time) 
                enemy = GetRandomEnemy(commonEnemyPrefabs);//TODO: change this to rareEnemyPrefabs
            else if (enemyTypeSpawn <= spawnChance.colorKeys[2].time)
                enemy = GetRandomEnemy(uncommonEnemyPrefabs);
            else if (enemyTypeSpawn <= spawnChance.colorKeys[3].time)
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
