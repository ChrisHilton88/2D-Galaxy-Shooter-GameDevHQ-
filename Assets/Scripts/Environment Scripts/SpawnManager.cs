using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private int _remainingEnemies;
    private int _currentWave;

    private bool _stopSpawning;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;

    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private GameObject[] _commonPowerups;
    [SerializeField] private GameObject[] _rarePowerups;

    WaitForSeconds _enemySpawnTimer = new WaitForSeconds(5.0f);

    UIManager _uiManager;

    Coroutine _co;


    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

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
        _co = StartCoroutine(SpawnPowerupsRoutine());

        while (!_stopSpawning)
        {
            yield return _enemySpawnTimer;
            SpawnRoutine();

            if (_remainingEnemies < 1)
            {
                _remainingEnemies = 0;
                StopCoroutine(_co);
                _currentWave++;
                NextWave();
                yield break;
            }
        }
    }

    IEnumerator SpawnPowerupsRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            WaitForSeconds _powerupTimer = new WaitForSeconds(Random.Range(3f, 7f));
            Vector3 powerupSpawnPos = new Vector3(Random.Range(-8f, 8f), 6f, 0);
            int randomNumber = Random.Range(0, 101);
            Debug.LogError(randomNumber);

            if (randomNumber >= 0 && randomNumber <= 30)
            {
                int randomRarePowerup = Random.Range(0, 3);
                Debug.LogError(randomRarePowerup);
                GameObject powerup = Instantiate(_rarePowerups[randomRarePowerup], powerupSpawnPos, Quaternion.identity);
                Debug.Log(powerup.gameObject.transform.name);
            }
            else if(randomNumber > 30 && randomNumber <= 100)
            {
                int randomCommonPowerup = Random.Range(0, 4);
                Debug.LogError(randomCommonPowerup);
                GameObject powerup = Instantiate(_commonPowerups[randomCommonPowerup], powerupSpawnPos, Quaternion.identity);
                Debug.Log(powerup.gameObject.transform.name);
            }
            else
            {
                Debug.Log("Not a valid number");
            }

            yield return _powerupTimer;
        }
    }




    void SpawnRoutine()
    {
        float _randomX = Random.Range(-8f, 8f);
        float _randomY = Random.Range(0f, 4f);
        int _randomEnemy = Random.Range(0, 3);

        Vector3 _normalEnemySpawn = new Vector3(_randomX, 6f, 0f);
        Vector3 _laserEnemySpawn = new Vector3(-9.5f, _randomY, 0f);
        Vector3 _teleportEnemySpawn = new Vector3(_randomX, 6f, 0f);

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
                StartCoroutine(_uiManager.WaveText(_currentWave));
                StartCoroutine(SpawningEnemiesRoutine(10));
                break;
            case 4:
                StartCoroutine(_uiManager.WaveText(_currentWave));
                StartCoroutine(SpawningEnemiesRoutine(15));
                break;
            case 5:
                StartCoroutine(_uiManager.WaveText(_currentWave));
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
