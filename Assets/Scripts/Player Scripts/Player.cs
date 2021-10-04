using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _playerLives = 3;
    private int _score;
    private int _shieldHits;
    private int _maxAmmo = 15;
    private int _minAmmo = 0;
    private int _ammoCount;
    private int _ammoShot = 1;

    private float _speed = 7.5f;
    private float _speedMultiplier = 2f;
    private float _minSpeed = 7.5f;
    private float _maxSpeed = 15f;
    private float _noSpeed = 0f;
    private float _canFire = 0;
    private float _fireRate = 0.25f;
    private float _xBoundary = 8.5f, _yBoundary = 4.5f;

    private bool _isTripleShotEnabled;
    private bool _isSpeedBoostEnabled;
    private bool _isShieldBoostEnabled;
    private bool _isMegaLaserEnabled;
    private bool _isNegativePickupEnabled;
    private bool _thrusterEngineCoroutinePlaying;
    private bool _isMagnetCooldownRoutinePlaying;

    private Vector3 _laserOffset = new Vector3(0f, 0.5f, 0f);
    private Vector3 _megaLaserOffset = new Vector3(0f, 5.25f, 0f);
    private Vector3 _tripleShotOffset = new Vector3(0f, 0.6f, 0f);

    Color _shieldColor;
    Color _thrusterBoostColor = new Color(1f, 1f, 1f, 1f);
    Color _mainEngine = new Color(1f, 1f, 1f, 0.75f);
    Color _NegativePowerup = new Color(1f, 1f, 1f, 0.5f);

    [SerializeField] AudioClip _laserShotClip;
    [SerializeField] AudioClip _emptyAmmoClip;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _megaLaserPrefab;
    [SerializeField] private GameObject _rightEngineThruster;
    [SerializeField] private GameObject _leftEngineThruster;
    [SerializeField] private GameObject _playerShield;

    AudioSource _audioSource;

    CameraShake _cameraShake;

    SpawnManager _spawnManager;

    SpriteRenderer _playerRend;
    SpriteRenderer _shieldSpriteRend;
    SpriteRenderer _thrustRend;

    ThrusterController _thrustCont;

    ThrusterEngine _thrusterEngine;

    UIManager _uiManager;

    WaitForSeconds _powerupRoutineThreeSeconds = new WaitForSeconds(3.0f);
    WaitForSeconds _powerupRoutineFourSeconds = new WaitForSeconds(4.0f);
    WaitForSeconds _powerupRoutineFiveSeconds = new WaitForSeconds(5.0f);


    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _playerRend = GetComponent<SpriteRenderer>();
        _shieldSpriteRend = transform.Find("Shield").GetComponentInChildren<SpriteRenderer>();
        _thrustRend = GameObject.Find("Thruster").GetComponentInChildren<SpriteRenderer>();
        _thrusterEngine = transform.Find("Thruster Engine").GetComponentInChildren<ThrusterEngine>();
        _thrustCont = GameObject.Find("Thruster").GetComponent<ThrusterController>();
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL in Player");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UIManager is NULL in Player");
        }

        if(_audioSource == null)
        {
            Debug.LogError("AudioClip is NULL in Player");
        }

        if(_playerRend == null)
        {
            Debug.LogError("SpriteRenderer is NULL in Player");
        }

        if(_shieldSpriteRend == null)
        {
            Debug.LogError("Shield is NULL in Player");
        }

        if(_thrustRend == null)
        {
            Debug.LogError("Thruster is NULL in Player");
        }

        if(_thrusterEngine == null)
        {
            Debug.LogError("ThrusterEngine is NULL in Player");
        }

        if(_thrustCont == null)
        {
            Debug.LogError("ThrusterController is NULL in Player");
        }

        if(_cameraShake == null)
        {
            Debug.LogError("CameraShake is NULL in Player");
        }

        _playerShield.SetActive(false);
        _thrusterEngine.gameObject.SetActive(false);
        _ammoCount = _maxAmmo;
    }

    void Update()
    {
        PlayerMovement();
        MagnetActivated();

        if(_isNegativePickupEnabled == false)
        {
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                if (_ammoCount > 0)
                {
                    FireLaser();
                    _uiManager.UpdateAmmoDisplay(_ammoCount, _maxAmmo);
                }
                else if (_ammoCount <= 0)
                {
                    _uiManager.OutOfAmmo();
                    _audioSource.clip = _emptyAmmoClip;
                    _audioSource.Play();
                    return;
                }
            }
        }
        else
        {
            return;
        }
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        direction.Normalize();

        if (_isSpeedBoostEnabled && _isNegativePickupEnabled == false)
        {
            transform.Translate(direction * _maxSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -_xBoundary, _xBoundary), Mathf.Clamp(transform.position.y, -_yBoundary, _yBoundary), 0);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotEnabled == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + _tripleShotOffset, Quaternion.identity);
            _ammoCount -= _ammoShot;
            CheckAmmoCount();
        }

        else if (_isMegaLaserEnabled == true)
        {
            if (GameObject.Find("Mega_Laser(Clone)") != null)
            {
                return;
            }
            else
            {
                GameObject _megaLas = Instantiate(_megaLaserPrefab, transform.position + _megaLaserOffset, Quaternion.identity);
                _megaLas.transform.parent = transform;
                Destroy(_megaLas, 3.0f);
            }
        }

        else
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
            _ammoCount -= _ammoShot;
            CheckAmmoCount();
        }

        _audioSource.clip = _laserShotClip;
        _audioSource.Play();
    }

    void CheckAmmoCount()
    {
        if (_ammoCount <= 0)
        {
            _ammoCount = _minAmmo;
        }
    }

    void ShieldBoostDeactivated()
    {
        _shieldHits = 0;
        _isShieldBoostEnabled = false;
        _playerShield.SetActive(false);
    }

    void ShieldDamage()
    {
        _shieldHits--;

        if (_shieldHits == 2)
        {
            _shieldColor = _shieldSpriteRend.color;
            _shieldColor.a = 0.66f;
            _shieldSpriteRend.color = _shieldColor;
        }
        else if (_shieldHits == 1)
        {
            _shieldColor = _shieldSpriteRend.color;
            _shieldColor.a = 0.33f;
            _shieldSpriteRend.color = _shieldColor;
        }
        else if (_shieldHits < 1)
        {
            ShieldBoostDeactivated();
        }
    }

    void MagnetActivated()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isMagnetCooldownRoutinePlaying == false)
        {
            StartMagnet();
        }
        else if (Input.GetKeyDown(KeyCode.R) && _isMagnetCooldownRoutinePlaying == true)
        {
            Debug.Log("Magnet is cooling down");
        }
    }

    void StartMagnet()
    {
        GameObject[] _powerupList = GameObject.FindGameObjectsWithTag("Powerup");

        if (_powerupList != null)
        {
            for (int i = 0; i < _powerupList.Length; i++)
            {
                Powerups power = _powerupList[i].GetComponent<Powerups>();
                power.Magnetise();
                StartCoroutine(MagnetCooldownRoutine(10f));
            }
        }

        if (_powerupList.Length == 0)
        {
            StartCoroutine(MagnetCooldownRoutine(5f));
        }
    }

    public void Damage()
    {
        if (_isShieldBoostEnabled == true)
        {
            ShieldDamage();
            return;
        }
        else
        {
            _playerLives--;
            _cameraShake.CallCameraShake();
        }

        switch (_playerLives)
        {
            case 3:
                break;
            case 2:
                _rightEngineThruster.SetActive(true);
                break;
            case 1:
                _leftEngineThruster.SetActive(true);
                break;
            case 0:
                _spawnManager.OnPlayerDeath();
                Destroy(gameObject);
                break;
            default:
                _spawnManager.OnPlayerDeath();
                Destroy(gameObject);
                Debug.Log("Invalid player lives");
                break;
        }

        _uiManager.UpdateLivesDisplay(_playerLives);
    }

    public void TripleShotActive()
    {
        _isTripleShotEnabled = true;
        StartCoroutine(TripleShotPowerupRoutine());
    }

    public void MegaLaserActive()
    {
        _isMegaLaserEnabled = true;
        StartCoroutine(MegaLaserPowerupRoutine());
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostEnabled = true;
        StartCoroutine(SpeedBoostPowerupRoutine());
    }

    public void ThrusterBoostActivated()
    {
        _speed *= _speedMultiplier;
        _thrustRend.color = _thrusterBoostColor;

        if(_speed >= _maxSpeed)
        {
            _speed = _maxSpeed;
        }
    }

    public void ThrusterBoostDeactivated()
    {
        if(_isNegativePickupEnabled == true)
        {
            _speed = _noSpeed;
            _thrustRend.color = _NegativePowerup;
            _playerRend.color = _NegativePowerup;
        }
        else
        {
            _speed = _minSpeed;
            _thrustRend.color = _mainEngine;
        }
    }

    public void ThrusterOverload()
    {
        if (_thrusterEngineCoroutinePlaying == false)
        {
            StartCoroutine(ThrusterEngineSmoke());
        }
        else
        {
            return;
        }
    }

    public void ShieldBoostActive()
    {
        _playerShield.SetActive(true);
        _shieldHits = 3;
        _shieldSpriteRend.color = new Color(1f, 1f, 1f, 1f);
        _isShieldBoostEnabled = true;
    }

    public void AmmoRefillActive()
    {
        _ammoCount = _maxAmmo;
        _uiManager.UpdateAmmoDisplay(_ammoCount, _maxAmmo);
    }

    public void HealthRefillActive()
    {
        if (_playerLives < 3)
        {
            switch (_playerLives)
            {
                case 1:
                    _leftEngineThruster.SetActive(false);
                    break;
                case 2:
                    _rightEngineThruster.SetActive(false);
                    break;
                case 3:
                    break;
                default:
                    Debug.Log("Invalid player life");
                    break;
            }

            _playerLives++;
            _uiManager.UpdateLivesDisplay(_playerLives);
        }
        else
        {
            return;
        }
    }

    public void NegativePickup()
    {
        _isNegativePickupEnabled = true;
        StartCoroutine(NegativePickupRoutine());
        StartCoroutine(_thrustCont.NegativePickupRoutine());
    }

    public void AddPoints(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    IEnumerator TripleShotPowerupRoutine()
    {
        yield return _powerupRoutineFiveSeconds;
        _isTripleShotEnabled = false;
    }

    IEnumerator SpeedBoostPowerupRoutine()
    {
        yield return _powerupRoutineFiveSeconds;
        _isSpeedBoostEnabled = false;
    }

    IEnumerator MegaLaserPowerupRoutine()
    {
        yield return _powerupRoutineThreeSeconds;
        _isMegaLaserEnabled = false;
    }

    IEnumerator ThrusterEngineSmoke()
    {
        _thrustRend.enabled = false;
        _thrusterEngineCoroutinePlaying = true;
        _thrusterEngine.gameObject.SetActive(true);
        _thrusterEngine.PlaySmokeAnimation();
        yield return _powerupRoutineThreeSeconds;
        _thrustRend.enabled = true;
        _thrusterEngineCoroutinePlaying = false;
        _thrusterEngine.gameObject.SetActive(false);
    }

    IEnumerator NegativePickupRoutine()
    {
        _playerRend.color = _NegativePowerup;
        _uiManager.EngineAndLasersDisabled();
        yield return _powerupRoutineFourSeconds;
        _playerRend.color = _mainEngine;
        _uiManager.EngineAndLasersEnabled();
        _isNegativePickupEnabled = false;
    }

    IEnumerator MagnetCooldownRoutine(float time)
    {
        _isMagnetCooldownRoutinePlaying = true;
        yield return new WaitForSeconds(time);
        _isMagnetCooldownRoutinePlaying = false;
    }
}   