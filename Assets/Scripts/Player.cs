using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _speed = 10f;
    private float _speedMultiplier = 1.5f;
    private float _minSpeed = 10f;
    private float _maxSpeed = 20f;

    private float _canFire = 0;
    private float _fireRate = 0.25f;

    private int _playerLives = 3;
    private int _score;
    private int _shieldHits;
    [SerializeField] private int _maxAmmo = 15;
    [SerializeField] private int _minAmmo = 0;
    [SerializeField] private int _ammoCount;
    [SerializeField] private int _ammoShot = 1;

    private bool _isTripleShotEnabled = false;
    private bool _isSpeedBoostEnabled = false;
    private bool _isShieldBoostEnabled = false;
    private bool _coroutinePlaying = false;

    private float _xBoundary = 8.5f, _yBoundary = 4.5f;

    private Vector3 _laserOffset = new Vector3(0, 0.5f, 0);

    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject tripleShotPrefab;
    [SerializeField] private GameObject rightEngineThruster;
    [SerializeField] private GameObject leftEngineThruster;
    [SerializeField] private GameObject playerShield;

    SpawnManager _spawnManager;

    UIManager _uiManager;

    ThrusterEngine _thrusterEngine;

    AudioSource _audioSource;

    SpriteRenderer _shieldSpriteRend;

    SpriteRenderer _thrustRend;

    CameraShake _cameraShake;

    [SerializeField] AudioClip _laserShotClip;
    [SerializeField] AudioClip _emptyAmmoClip;

    Color _shieldColor;
    Color _thrusterBoostColor = new Color(1f, 1f, 1f, 1f);
    Color _mainEngine = new Color(1f, 1f, 1f, 0.75f);


    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shieldSpriteRend = transform.Find("Shield").GetComponentInChildren<SpriteRenderer>();
        _thrustRend = GameObject.Find("Thruster").GetComponentInChildren<SpriteRenderer>();
        _thrusterEngine = transform.Find("Thruster Engine").GetComponentInChildren<ThrusterEngine>();
        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager not found in Player");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UIManager not found in Canvas game object");
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio clip is null");
        }

        if(_shieldSpriteRend == null)
        {
            Debug.Log("Shield is null");
        }

        if(_thrustRend == null)
        {
            Debug.Log("Thruster is null");
        }

        if(_thrusterEngine == null)
        {
            Debug.Log("thruster engine is null");
        }

        if(_cameraShake == null)
        {
            Debug.Log("Camera Shake is null in Player script");
        }

        playerShield.SetActive(false);
        _thrusterEngine.gameObject.SetActive(false);
        _ammoCount = _maxAmmo;
    }

    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_ammoCount > 0)
            {
                FireLaser();
            }
            else if(_ammoCount <= 0)
            {
                _uiManager.OutOfAmmo();
                _audioSource.clip = _emptyAmmoClip;
                _audioSource.Play();
                return;
            }
        }
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -_xBoundary, _xBoundary), Mathf.Clamp(transform.position.y, -_yBoundary, _yBoundary), 0);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotEnabled == true)
        {
            Instantiate(tripleShotPrefab, transform.position, Quaternion.identity);
            _ammoCount -= _ammoShot;
            CheckAmmoCount();
        }

        else
        {
            Instantiate(laserPrefab, transform.position + _laserOffset, Quaternion.identity);
            _ammoCount -= _ammoShot;
            CheckAmmoCount();
        }

        _audioSource.clip = _laserShotClip;
        _audioSource.Play();
    }

    private void CheckAmmoCount()
    {
        if (_ammoCount <= 0)
        {
            _ammoCount = _minAmmo;
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
                rightEngineThruster.SetActive(true);
                break;
            case 1:
                leftEngineThruster.SetActive(true);
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

    public void SpeedBoostActive()
    {
        _isSpeedBoostEnabled = true;

        // Need to fix up the speed multiplier
        if (_speed >= _maxSpeed)
        {
            _speed = _maxSpeed;
        }

        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerupRoutine());
    }

    public void ThrusterBoostActivated()
    {
        _speed = _maxSpeed;
        _thrustRend.color = _thrusterBoostColor;
    }

    public void ThrusterBoostDeactivated()
    {
        _speed = _minSpeed;
        _thrustRend.color = _mainEngine;
    }

    public void ThrusterOverload()
    {
        if (_coroutinePlaying == false)
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
        playerShield.SetActive(true);
        _shieldHits = 3;
        _shieldSpriteRend.color = new Color(1f, 1f, 1f, 1f);
        _isShieldBoostEnabled = true;
    }

    public void AmmoRefillActive()
    {
        _ammoCount = _maxAmmo;
    }

    void ShieldBoostDeactivated()
    {
        _shieldHits = 0;
        _isShieldBoostEnabled = false;
        playerShield.SetActive(false);
    }

    IEnumerator TripleShotPowerupRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotEnabled = false;
    }

    IEnumerator SpeedBoostPowerupRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
        _isSpeedBoostEnabled = false;
    }

    public void AddPoints(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
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

    IEnumerator ThrusterEngineSmoke()
    {
        _thrustRend.enabled = false;
        _coroutinePlaying = true;
        _thrusterEngine.gameObject.SetActive(true);
        _thrusterEngine.PlaySmokeAnimation();
        yield return new WaitForSeconds(3.0f);
        _thrustRend.enabled = true;
        _coroutinePlaying = false;
        _thrusterEngine.gameObject.SetActive(false);
    }
}   