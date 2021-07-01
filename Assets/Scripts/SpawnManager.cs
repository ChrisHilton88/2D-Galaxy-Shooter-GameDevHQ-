using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private GameObject tripleShotPrefab;

    private bool stopSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }


    IEnumerator SpawnEnemiesRoutine()
    {
        while (stopSpawning == false)
        {
            Vector3 enemySpawnPos = new Vector3(Random.Range(-8f, 8f), 6f, 0);
            GameObject newEnemy = Instantiate(enemyPrefab, enemySpawnPos, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(3.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while(stopSpawning == false)
        {
            Vector3 powerupSpawnPos = new Vector3(Random.Range(-8f, 8f), 6f, 0);
            Instantiate(tripleShotPrefab, powerupSpawnPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    public void OnPlayerDeath()
    {
        Debug.Log("OnPlayerDeath() true");
        stopSpawning = true;
    }
}
