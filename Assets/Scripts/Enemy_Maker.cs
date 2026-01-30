using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Maker : MonoBehaviour
{
    public List<GameObject> EnemyPrefabs = new List<GameObject>();

    public float spawnInterval = 2f;

    
    public Transform spawnPoint;

    public int maxEnemiesToSpawn = 0; 
    private int enemiesSpawned = 0;

    public GameObject target; 
    public game_maneger game_Maneger;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (maxEnemiesToSpawn == 0 || enemiesSpawned < maxEnemiesToSpawn)
        {
            
            int randomEnemyIndex = Random.Range(0, EnemyPrefabs.Count);
            GameObject enemyPrefab = EnemyPrefabs[randomEnemyIndex];

            
            Vector2 spawnPosition = spawnPoint.position;

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

           
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
            enemyScript.gameManager = game_Maneger;
           

            enemiesSpawned++;

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
