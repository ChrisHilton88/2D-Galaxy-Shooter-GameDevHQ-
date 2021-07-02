using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 12.5f;
    [SerializeField] private float _speedMultiplier = 1.5f;
    [SerializeField] private float _maxSpeed = 25;

    private float _xBoundary = 8.5f, _yBoundary = 4.5f;

    private Vector3 _laserOffset = new Vector3(0, 1, 0);

    public GameObject laserPrefab;
    public GameObject tripleShotPrefab;

    private float _canFire = 0;
    private float _fireRate = 0.25f;

   [SerializeField] private int _playerLives = 3;

    SpawnManager spawnManager;

    [SerializeField] private GameObject playerShield;

    [SerializeField] private bool _isTripleShotEnabled = false;
    [SerializeField] private bool _isSpeedBoostEnabled = false;
    [SerializeField] private bool _isShieldBoostEnabled = false;


    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();  

        if (spawnManager == null)
        {
            Debug.LogError("SpawnManager not found in Player");
        }
    }

    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
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

        if(_isTripleShotEnabled == true)
        {
            Instantiate(tripleShotPrefab, transform.position, Quaternion.identity);
        }

        else
        {
            Instantiate(laserPrefab, transform.position + _laserOffset, Quaternion.identity);
        }
    }

    public void Damage()
    {
        if (_isShieldBoostEnabled == true)
        {
            _isShieldBoostEnabled = false;
            playerShield.SetActive(false);
            return;           
        }

        _playerLives--;

        if (_playerLives < 1)
        {
            Destroy(this.gameObject);
            spawnManager.OnPlayerDeath();
        }
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
        if(_speed >= _maxSpeed)
        {
            _speed = _maxSpeed;
        }

        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerupRoutine());
    }

    public void ShieldBoostActive()
    {
        _isShieldBoostEnabled = true;
        playerShield.SetActive(true);
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
}
