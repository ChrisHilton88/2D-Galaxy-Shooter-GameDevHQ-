using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 12.5f;
    [SerializeField] private float _speedMultiplier = 1.5f;
    [SerializeField] private float _thrusterSpeedMultiplier = 1.5f;
    [SerializeField] private float _minSpeed = 12.5f;
    [SerializeField] private float _maxSpeed = 25;

    private float _xBoundary = 8.5f, _yBoundary = 4.5f;

    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject tripleShotPrefab;
    [SerializeField] private GameObject rightEngineThruster;
    [SerializeField] private GameObject leftEngineThruster;

    AudioSource audioSource;

    private float _canFire = 0;
    private float _fireRate = 0.25f;

    private Vector3 laserOffset = new Vector3(0, 0.5f, 0);

    [SerializeField] private int _playerLives = 3;
    [SerializeField] private int _score;
    [SerializeField] private int _shieldHits;

    SpawnManager spawnManager;

    UIManager uiManager;

    [SerializeField] private GameObject playerShield;

    [SerializeField] private bool _isTripleShotEnabled = false;
    [SerializeField] private bool _isSpeedBoostEnabled = false;
    [SerializeField] private bool _isShieldBoostEnabled = false;

    Color shieldColor;

    SpriteRenderer shieldSpriteRend;


    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        audioSource = GameObject.Find("Laser_Shot_Audio_Clip").GetComponent<AudioSource>();
        shieldSpriteRend = transform.Find("Shield").GetComponentInChildren<SpriteRenderer>();

        if (spawnManager == null)
        {
            Debug.LogError("SpawnManager not found in Player");
        }

        if (uiManager == null)
        {
            Debug.LogError("UIManager not found in Canvas game object");
        }

        if(audioSource == null)
        {
            Debug.LogError("Audio clip is null");
        }

        if(shieldSpriteRend == null)
        {
            Debug.Log("Shield is null");
        }

        playerShield.SetActive(false);
    }

    void Update()
    {
        PlayerMovement();
        ThrusterBoost();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void ThrusterBoost()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed *= _thrusterSpeedMultiplier; 
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _minSpeed;
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
        }

        else
        {
            Instantiate(laserPrefab, transform.position + laserOffset, Quaternion.identity);
        }

        audioSource.Play();
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
                spawnManager.OnPlayerDeath();
                Destroy(gameObject);
                break;
            default:
                Debug.Log("Invalid player lives");
                break;
        }

        uiManager.UpdateLivesDisplay(_playerLives);
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

    public void ShieldBoostActive()
    {
        _shieldHits = 3;
        shieldSpriteRend.color = new Color(1f, 1f, 1f, 1f);
        _isShieldBoostEnabled = true;
        playerShield.SetActive(true);
        Debug.Log("Shield boost is now active");
    }

    void ShieldBoostDeactivated()
    {
        _shieldHits = 0;
        _isShieldBoostEnabled = false;
        playerShield.SetActive(false);
        Debug.Log("Shield boost is now DEACTIVATED");
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
        uiManager.UpdateScore(_score);
    }

    void ShieldDamage()
    {
        _shieldHits--;

        if (_shieldHits == 3)
        {
            shieldColor = shieldSpriteRend.color;
            shieldColor.a = 1f;
            shieldSpriteRend.color = shieldColor;
            Debug.Log("Alpha is 1");
        }
        else if (_shieldHits == 2)
        {
            shieldColor = shieldSpriteRend.color;
            shieldColor.a = 0.66f;
            shieldSpriteRend.color = shieldColor;
            Debug.Log("Alpha is 0.66f");
        }
        else if (_shieldHits == 1)
        {
            shieldColor = shieldSpriteRend.color;
            shieldColor.a = 0.33f;
            shieldSpriteRend.color = shieldColor;
            Debug.Log("Alpha is 0.33f");
        }
        else if (_shieldHits < 1)
        {
            ShieldBoostDeactivated();
        }
    }

}   