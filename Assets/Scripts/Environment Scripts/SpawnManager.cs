using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private int _remainingEnemies;
    private int _currentWave;

    private bool _stopSpawning;

    Vector3 _bossSpawn = new Vector3(0f, 8f, 0f);

    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private GameObject[] _commonPowerups;
    [SerializeField] private GameObject[] _rarePowerups;

    Coroutine _co;

    HomingMissile _homingMissile;

    UIManager _uiManager;

    WaitForSeconds _enemySpawnTimer = new WaitForSeconds(3.0f);


    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _homingMissile = GameObject.Find("Player").GetComponent<HomingMissile>();

        if(_uiManager == null)
        {
            Debug.LogError("UIManager is NULL in SpawnManager");
        }

        if(_homingMissile == null)
        {
            Debug.LogError("HomingMissile is NULL in SpawnManager");
        }

        _currentWave = 0;
    }

    void Update()
    {
        _uiManager.UpdateEnemiesRemaining(_remainingEnemies);
    }

    void SpawnRoutine()
    {
        float _randomX = Random.Range(-8f, 8f);
        float _randomY = Random.Range(0f, 4f);
        int _randomEnemy = Random.Range(0, 3);

        Vector3 _normalEnemySpawn = new Vector3(_randomX, 6f, 0f);
        Vector3 _laserEnemySpawn = new Vector3(-9.5f, _randomY, 0f);
        Vector3 _teleportEnemySpawn = new Vector3(_randomX, 6f, 0f);

        if (_currentWave == 6)
        {
            StartCoroutine(_uiManager.StartSliderBars());
            GameObject _bossEnemy = Instantiate(_enemies[3], _bossSpawn, Quaternion.identity);
            _bossEnemy.transform.parent = _enemyContainer.transform;
        }
        else if (_currentWave < 6)
        {
            switch (_randomEnemy)
            {
                case 0:
                    GameObject _newNormalEnemy = Instantiate(_enemies[0], _normalEnemySpawn, Quaternion.identity);
                    _newNormalEnemy.transform.parent = _enemyContainer.transform;
                    break;
                case 1:
                    GameObject _newTeleportEnemy = Instantiate(_enemies[1], _teleportEnemySpawn, Quaternion.identity);
                    _newTeleportEnemy.transform.parent = _enemyContainer.transform;
                    break;
                case 2:
                    GameObject _newLaserEnemy = Instantiate(_enemies[2], _laserEnemySpawn, Quaternion.identity);
                    _newLaserEnemy.transform.parent = _enemyContainer.transform;
                    break;
                default:
                    break;
            }

            _remainingEnemies--;
        }
    }

    void NextWave()
    {
        switch (_currentWave)
        {
            case 1:
                StartCoroutine(_uiManager.WaveText(_currentWave));
                StartCoroutine(SpawningEnemiesRoutine(3));
                break;
            case 2:
                StartCoroutine(_uiManager.WaveText(_currentWave));
                StartCoroutine(SpawningEnemiesRoutine(6));
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
            case 6:
                StartCoroutine(_uiManager.WaveText(_currentWave));
                StartCoroutine(SpawningEnemiesRoutine(1));
                break;
            case 7:
                _uiManager.CongraulationsSequence();
                break;
            default:
                Debug.Log("Invalid wave");
                break;
        }
    }

    public void StartSpawning()
    {
        _currentWave++;
        _homingMissile.IncreaseWaveNumber();
        NextWave();
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void FinishingWaveSequence()
    {
        _remainingEnemies = 0;
        StopCoroutine(_co);
        _currentWave++;
        NextWave();
    }

    IEnumerator SpawningEnemiesRoutine(int _remainingEnemiesToSpawn)
    {
        _remainingEnemies = _remainingEnemiesToSpawn;
        _co = StartCoroutine(SpawnPowerupsRoutine());

        while (!_stopSpawning && _currentWave < 6)
        {
            yield return _enemySpawnTimer;
            SpawnRoutine();

            if (_remainingEnemies < 1)
            {
                _remainingEnemies = 0;
                StopCoroutine(_co);
                _currentWave++;
                NextWave();
                _homingMissile.IncreaseWaveNumber();
                yield break;
            }
        }

        if(_currentWave == 6)
        {
            SpawnRoutine();
        }
    }

    IEnumerator SpawnPowerupsRoutine()
    {
        yield return new WaitForSeconds(3f);

        while (!_stopSpawning)
        {
            WaitForSeconds _powerupTimer = new WaitForSeconds(Random.Range(2f, 4f));
            Vector3 powerupSpawnPos = new Vector3(Random.Range(-8f, 8f), 6f, 0);
            int randomNumber = Random.Range(0, 101);

            if (randomNumber >= 0 && randomNumber <= 30)
            {
                int randomRarePowerup = Random.Range(0, 3);
                GameObject _commonPowerup = Instantiate(_rarePowerups[randomRarePowerup], powerupSpawnPos, Quaternion.identity);
            }
            else if(randomNumber > 30 && randomNumber <= 100)
            {
                int randomCommonPowerup = Random.Range(0, 4);
                GameObject _rarePowerup = Instantiate(_commonPowerups[randomCommonPowerup], powerupSpawnPos, Quaternion.identity);
            }
            else
            {
                Debug.Log("Not a valid number");
            }

            yield return _powerupTimer;
        }
    }
}
