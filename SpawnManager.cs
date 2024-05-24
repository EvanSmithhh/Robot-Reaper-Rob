using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] doors;
    public GameObject enemyPrefab;
    public Transform enemies;

    private int enemyNumber;
    private float spawnRate = 0.7f;
    private float spawnDelay = 2;
    private bool increasedSpawn = false;

    public int countDown = 5;
    private int spawnOffset;
    private int doorDec;

    public GameManager gameManager;
    
    void Start()
    {
        StartCoroutine(EnemySpawner());
        StartCoroutine(IncreaseSpawnRate());
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown == 0 && !increasedSpawn)
        {
            spawnRate = 0.45f;
            increasedSpawn = true;
        }

        enemyNumber = enemies.childCount;
    }

    // Spawns an enemy at a random door
    void SpawnEnemies()
    {
        doorDec = (Random.Range(0, doors.Length));

        if (doorDec > 2)
        {
            spawnOffset = 2;
        }
        else
        {
            spawnOffset = -2;
        }

        Vector3 spawnLocation = new (doors[doorDec].transform.position.x + spawnOffset, 1.26f, doors[doorDec].transform.position.z) ;

        Instantiate(enemyPrefab, spawnLocation, Quaternion.identity,  enemies);
    }

    private IEnumerator EnemySpawner()
    {
        yield return new WaitForSeconds(spawnDelay);
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            if (enemyNumber <= 40 && !gameManager.gameOver)
            {
                SpawnEnemies();
            }
        }
    }

    private IEnumerator IncreaseSpawnRate()
    {
        while (countDown > 0)
        {
            yield return new WaitForSeconds(1);
            countDown--;
            spawnRate -= 0.001f;
        }
    }

}
