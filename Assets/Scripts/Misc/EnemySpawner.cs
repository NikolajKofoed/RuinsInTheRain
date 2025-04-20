using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private int enemyAmountPerSpawn = 1;
    [SerializeField] private float spawnIntervalMin = 1;
    [SerializeField] private float spawnIntervalMax = 3;
    [SerializeField] private float amountSpawned;
    [SerializeField] private float spawnDelayPerEnemy;

    private bool canSpawn = true;
    private float currentAmountSpawned = 0;

    private void Update()
    {
        if (currentAmountSpawned < amountSpawned && canSpawn || amountSpawned == 0 && canSpawn)
        {
            canSpawn = false;
            StartCoroutine(SpawnRoutine());
        }
    }

    private IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < enemyAmountPerSpawn; i++)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            currentAmountSpawned++;

            if (currentAmountSpawned >= amountSpawned && amountSpawned != 0)
            {
                break;
            }

            // Wait before next enemy in the same batch
            yield return new WaitForSeconds(spawnDelayPerEnemy);
        }

        // Wait between batches
        yield return new WaitForSeconds(Random.Range(spawnIntervalMin, spawnIntervalMax));
        canSpawn = true;
    }
}
