using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossBehaviour : MonoBehaviour
{
    private int _randomNumber;

    private float _bossSpeed;
    private float _bossSpeedMultiplier = 3;
    private float _bossShieldPercent = 100f;
    private float _bossShieldMinPercent = 0f;
    private float _bossShieldMaxPercent = 100f;
    private float _bossHealthPercent = 100f;
    private float _minBossHealth = 0f;
    private float _canFireLaser = 5f;
    private float _canFireQuadLasers = 10f;
    private float _canFireMines = 7f;
    private float _fireRateLasers = 1.5f;
    private float _fireRateQuadLasers = 10f;
    private float _fireRateMines;
    private float _movementBoundary = 6f;

    private bool _startMovement = true;
    private bool _shieldActivated = true;
    private bool _isShieldDownRoutinePlaying;
    private bool _moveRight;
    private bool _shieldSliderFound;
    private bool _healthSliderFound;
    private bool _isBossActive;
    private bool _gameOver;

    Vector3 _startSpawn = new Vector3(0f, 8f, 0f);
    Vector3 _middlePos = new Vector3(0f, 1.3f, 0f);
    Vector3 _rightPos = new Vector3(6f, 1.3f, 0f);
    Vector3 _leftPos = new Vector3(-6f, 1.3f, 0f);
    Vector3 _bossLaserOffset = new Vector3(0f, -2f, 0f);

    [SerializeField] private GameObject _bossShield;
    [SerializeField] private GameObject _bossQuadLaserPrefab;
    [SerializeField] private GameObject _minePrefab;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _explosionPrefab;

    GameManager _gameManager;

    Player _player;

    Slider _bossShieldSlider;
    Slider _bossHealthSlider;

    SpawnManager _spawnManager;

    UIManager _uiManager;


    void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL in BossBehaviour");
        }

        if(_player == null)
        {
            Debug.LogError("Player is NULL in BossBehaviour");
        }

        if(_uiManager == null)
        {
            Debug.LogError("UIManager is NULL in BossBehaviour");
        }

        if(_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL in BossBehaviour");
        }

        _canFireLaser = Time.time + 5f;
        _canFireQuadLasers = Time.time + 7.5f;
        _canFireMines = Time.time + 6f;
        StartCoroutine(SliderBarsRoutine());
        _bossHealthPercent = 100f;
        _bossShieldPercent = 100f;
    }

    void Update()
    {
        BossQuadFireCooldown();
        MineCooldown();
        LaserCooldown();

        if (_startMovement)
        {
            _bossSpeed += 0.25f * Time.deltaTime;
            transform.position = Vector3.Lerp(_startSpawn, _middlePos, _bossSpeed);

            if(transform.position.y == 1.3f)
            {
                _isBossActive = true;
                _uiManager.BossBattleText("Boss Shield");
                _moveRight = true;
                _startMovement = false;
            }
        }
        else if(!_startMovement)
        {
            Movement();
        }

        if(_bossShieldPercent <= 0 && !_isShieldDownRoutinePlaying)
        {
            StartCoroutine(ShieldDownRoutine());
        }

        if(_bossHealthPercent <= 0 && _gameOver == false)
        {
            _gameOver = true;
            _player.AddPoints(500);
            _gameManager.GameOver();
            _spawnManager.FinishingWaveSequence();   
            _uiManager.BossBattleTextSwitchOff();
            _bossHealthSlider.gameObject.SetActive(false);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if(_shieldSliderFound == true && _healthSliderFound == true)
        {
            _bossShieldSlider.value = _bossShieldPercent;
            _bossHealthSlider.value = _bossHealthPercent;
        }
    }

    void Movement()
    {
        if(_moveRight)
        {
            Vector3 dir = transform.position - _rightPos;
            dir = -dir.normalized;
            transform.position += dir * _bossSpeed * _bossSpeedMultiplier * Time.deltaTime;

            if (transform.position.x > _movementBoundary)
            {
                _moveRight = false;
            }
        }
        else if(!_moveRight)
        {
            Vector3 dir = transform.position - _leftPos;
            dir = -dir.normalized;
            transform.position += dir * _bossSpeed * _bossSpeedMultiplier * Time.deltaTime;

            if (transform.position.x < -_movementBoundary)
            {
                _moveRight = true;
            }
        }
    }

    void LaserCooldown()
    {
        if (Time.time > _canFireLaser)
        {
            _canFireLaser = Time.time + _fireRateLasers;
            LaserFire();
        }
    }

    void LaserFire()
    {
        GameObject _bossLaser = Instantiate(_laserPrefab, transform.position + _bossLaserOffset, Quaternion.identity);
        Laser[] lasers = _bossLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser(true);
        }

        _bossShieldPercent -= 2f;
        _bossShieldPercent -= 2f;
    }

    void BossQuadFireCooldown()
    {
        if (Time.time > _canFireQuadLasers)
        {
            _canFireQuadLasers = Time.time + _fireRateQuadLasers;
            QuadLaserFire();
        }
    }

    void QuadLaserFire()
    {
        GameObject _bossQuadLasers = Instantiate(_bossQuadLaserPrefab, transform.position, Quaternion.identity);
        _bossQuadLasers.transform.parent = transform;
        _bossShieldPercent -= 10f;
        Destroy(_bossQuadLasers, 0.25f);
    }

    void MineCooldown()
    {
        if (Time.time > _canFireMines)
        {
            _fireRateMines = Random.Range(7f, 10f);
            _canFireMines = Time.time + _fireRateMines;
            DropMine();
        }
    }

    void DropMine()
    {
        GameObject _mine = Instantiate(_minePrefab, transform.position, Quaternion.identity);
        _bossShieldPercent -= 10f;
        Destroy(_mine, 3f);
    }

    void BossShieldDamage(float _shieldAmount)
    {
        _bossShieldPercent -= _shieldAmount;

        if (_bossShieldPercent <= 0)
        {
            _bossShieldPercent = _bossShieldMinPercent;
        }
    }

    void BossHealthDamage(float _damageAmount)
    {
        _bossHealthPercent -= _damageAmount;

        if (_bossHealthPercent <= 0)
        {
            _bossHealthPercent = _minBossHealth;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            if (_isBossActive)
            {
                if (_shieldActivated)
                {
                    BossShieldDamage(-15f);
                    if (_bossShieldPercent > _bossShieldMaxPercent)
                    {
                        _bossShieldPercent = _bossShieldMaxPercent;
                    }

                    Destroy(other.gameObject);
                }
                else
                {
                    BossHealthDamage(5f);
                    Destroy(other.gameObject);
                }
            }
            else
            {
                return;
            }
        }

        if(other.tag == "Missile")
        {
            if (_isBossActive)
            {
                if (_shieldActivated)
                {
                    BossShieldDamage(10f);
                }
                else
                {
                    BossHealthDamage(10f);
                }
            }
            else
            {
                return;
            }
        }

        if (other.tag == "MegaLaser")
        {
            if (_isBossActive)
            {
                if (_shieldActivated)
                {
                    BossShieldDamage(2f);
                }
                else
                {
                    BossHealthDamage(2f);
                }
            }
            else
            {
                return;
            }
        }

        if (other.tag == "Player")
        {
            _player.Damage();
        }
    }

    IEnumerator ShieldDownRoutine()
    {
        _uiManager.BossBattleText("Boss Health");
        _isShieldDownRoutinePlaying = true;
        _shieldActivated = false;
        _bossShield.SetActive(false);
        _bossShieldSlider.gameObject.SetActive(false);
        _bossHealthSlider.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        _uiManager.BossBattleText("Boss Shield");
        _bossShieldPercent = 100f;
        _shieldActivated = true;
        _bossShield.SetActive(true);
        _bossShieldSlider.gameObject.SetActive(true);
        _bossHealthSlider.gameObject.SetActive(false);
        _isShieldDownRoutinePlaying = false;
    }

    IEnumerator SliderBarsRoutine()
    {
        yield return new WaitForSeconds(4.1f);
        _bossShieldSlider = GameObject.Find("Boss_Shield_Bar").GetComponent<Slider>();
        _bossHealthSlider = GameObject.Find("Boss_Health_Bar").GetComponent<Slider>();

        if(_bossShieldSlider != null)
        {
            _shieldSliderFound = true;
        }

        if (_bossHealthSlider != null)
        {
            _healthSliderFound = true;
        }
    }
}
