using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int _remainingEnemies;
    [SerializeField] private int _currentWave;

    private float _randomX;
    private float _randomY;

    private bool _stopSpawning;

    Vector3 _normalEnemySpawn;
    Vector3 _laserEnemySpawn;
    Vector3 _teleportEnemySpawn;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;

    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private GameObject[] powerups;

    WaitForSeconds _enemySpawnTimer = new WaitForSeconds(5.0f);

    GameManager _gameManager;

    UIManager _uiManager;

    // Asteroid gets destroyed and calls the method to increase the current wave from 0 to 1
    // Then we call the NextWave() method and it runs through a switch statement of currentwave numbers to create a spawning coroutine

    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_gameManager == null)
        {
            Debug.Log("GameManager is null in SpawnManager");
        }

        if(_uiManager == null)
        {
            Debug.Log("UIManager is NULL in SpawnManager");
        }

        _currentWave = 0;
    }

    void Update()
    {
        _uiManager.UpdateEnemiesRemaining(_remainingEnemies);
    }

    public void StartSpawning()
    {
        _currentWave++;
        NextWave();
    }

    IEnumerator SpawningEnemiesRoutine(int _remainingEnemiesToSpawn)
    {
        _remainingEnemies = _remainingEnemiesToSpawn;
        StartCoroutine(SpawnCommonPowerupsRoutine());
        yield return new WaitForSeconds(2.0f);

        while (!_stopSpawning)
        {
            yield return _enemySpawnTimer;
            SpawnRoutine();
            Debug.Log("Spawning Wave " + _currentWave);

            if (_remainingEnemies < 1)
            {
                _remainingEnemies = 0;
                _currentWave++;
                NextWave();
                yield break;
            }
        }
    }

    IEnumerator SpawnCommonPowerupsRoutine()
    {
        while (!_stopSpawning)
        {
            WaitForSeconds _commonPowerupTimer = new WaitForSeconds(Random.Range(3f, 7f));
            Debug.Log(_commonPowerupTimer);
            Vector3 powerupSpawnPos = new Vector3(Random.Range(-8f, 8f), 6f, 0);
            int randomPowerup = Random.Range(0, 7);
            GameObject powerup = Instantiate(powerups[randomPowerup], powerupSpawnPos, Quaternion.identity);
            yield return _commonPowerupTimer;
        }
    }

    void SpawnRoutine()
    {
        _randomX = Random.Range(-8f, 8f);
        _randomY = Random.Range(0f, 4f);

        int _randomEnemy = Random.Range(0, 3);

        _normalEnemySpawn = new Vector3(_randomX, 6f, 0f);
        _laserEnemySpawn = new Vector3(-9.5f, _randomY, 0f);
        _teleportEnemySpawn = new Vector3(Random.Range(-8f, 8f), 6f, 0f);

        switch (_randomEnemy)
        {
            case 0:
                GameObject _newNormalEnemy = Instantiate(_enemies[0], _normalEnemySpawn, Quaternion.identity);
                _newNormalEnemy.transform.parent = enemyContainer.transform;
                break;
            case 1:
                GameObject _newTeleportEnemy = Instantiate(_enemies[1], _teleportEnemySpawn, Quaternion.identity);
                _newTeleportEnemy.transform.parent = enemyContainer.transform;
                break;
            case 2:
                GameObject _newLaserEnemy = Instantiate(_enemies[2], _laserEnemySpawn, Quaternion.identity);
                _newLaserEnemy.transform.parent = enemyContainer.transform;
                break;
            default:
                break;
        }

        _remainingEnemies--;
    }

    void NextWave()
    {
        switch (_currentWave)
        {
            case 1:
                StartCoroutine(_uiManager.WaveText(_currentWave));
                StartCoroutine(SpawningEnemiesRoutine(5));
                break;
            case 2:
                StartCoroutine(_uiManager.WaveText(_currentWave));
                StartCoroutine(SpawningEnemiesRoutine(7));
                break;
            case 3:
                StartCoroutine(SpawningEnemiesRoutine(10));
                break;
            case 4:
                StartCoroutine(SpawningEnemiesRoutine(15));
                break;
            case 5:
                StartCoroutine(SpawningEnemiesRoutine(20));
                break;
            default:
                Debug.Log("Invalid wave");
                break;
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
