using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float spawnIntervalMin = 1;
    [SerializeField] private float spawnIntervalMax = 3;

    private bool canSpawn = true;

    private void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;
            StartCoroutine(SpawnRoutine());
        }
    }

    private IEnumerator SpawnRoutine()
    {
        Instantiate(enemy, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(spawnIntervalMin,spawnIntervalMax));
        canSpawn = true;
    }
}
