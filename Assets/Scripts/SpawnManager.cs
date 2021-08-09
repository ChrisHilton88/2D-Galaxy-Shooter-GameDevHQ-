using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;

    [SerializeField] private GameObject[] powerups;  

    private bool _stopSpawning = false;


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemiesRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }


    IEnumerator SpawnEnemiesRoutine()
    {
        yield return new WaitForSeconds(2.633f);

        while (_stopSpawning == false)
        {
            Vector3 enemySpawnPos = new Vector3(Random.Range(-8f, 8f), 6f, 0);
            GameObject newEnemy = Instantiate(enemyPrefab, enemySpawnPos, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(3.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2.633f);

        while (_stopSpawning == false)
        {
            yield return null;
            Vector3 powerupSpawnPos = new Vector3(Random.Range(-8f, 8f), 6f, 0);
            int randomPowerup = Random.Range(0, 3);
            GameObject powerup = Instantiate(powerups[randomPowerup], powerupSpawnPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    public void OnPlayerDeath()
    {      
        _stopSpawning = true;
    }
}
