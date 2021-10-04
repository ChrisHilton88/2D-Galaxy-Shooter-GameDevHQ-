using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _enemySpeed = 3;
    private float _minEnemySpeed = 3;
    private float _ramSpeed = 10f;
    private float _fireRate;
    private float _canFire = 2f;
    private float _rayPowerupDistance = 6f;
    private float _ramRayDistance = 4f;

    private bool _powerupShotTrigger;
    private bool _playerShotTrigger;
    private bool _enemyShootUp;
    private bool _ramPlayerCoroutine;
    private bool _isCoroutinePlaying;
    //[SerializeField] private bool isAlive;

    Vector3 _downLaserOffset = new Vector3(0f, -0.25f, 0f);
    Vector3 _downRayOffset = new Vector3(0, -0.6f, 0);
    Vector3 _upRayOffset = new Vector3(0f, 0.5f, 0f);
    Vector3 _enemyShootUpOffset = new Vector3(0f, 1.50f, 0f);

    [SerializeField] private GameObject _enemyLaser;

    Animator _animator;

    AudioSource _audioSource;

    Player _player;

    SpawnManager _spawnManager;

    WaitForSeconds _ramTime = new WaitForSeconds(0.5f);
    WaitForSeconds _cooldownTimer = new WaitForSeconds(5.0f);


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL in Enemy");
        }

        if(_audioSource == null)
        {
            Debug.LogError("AudioSource is NULL in Enemy");
        }

        if(_animator == null)
        {
            Debug.LogError("Animator is NULL in Enemy");
        }

        if(_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NUL in Enemy");
        }

        //isAlive = true;
       // StartCoroutine(EnemyLaserRoutine());
    }

    void Update()
    {
        CalculateMovement();
        EnemyFire();
        RayCastPowerupShoot();
        RayCastRamPlayer();
        RayCastPlayerAboveShoot();
    }

    void RayCastPowerupShoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + _downRayOffset, -transform.up, _rayPowerupDistance);
        if (hit)
        {
            if (hit.collider.tag == "Powerup")
            {
                if (_powerupShotTrigger == false && _isCoroutinePlaying == false)
                {
                    LaserFire();
                    StartCoroutine(CooldownRoutine(_powerupShotTrigger));
                }
            }
        }
    }

    void RayCastPlayerAboveShoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + _upRayOffset, transform.up, _rayPowerupDistance);
        if (hit)
        {
            if (hit.collider.tag == "Player")
            {
                if (_playerShotTrigger == false && _isCoroutinePlaying == false)
                {
                    _enemyShootUp = true;
                    LaserFire();
                    StartCoroutine(CooldownRoutine(_playerShotTrigger));
                }
            }
        }
    }

    void RayCastRamPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + _downRayOffset, -transform.up, _ramRayDistance);
        if (hit)
        {
            if(hit.collider.tag == "Player")
            {
                if(_ramPlayerCoroutine == false && _isCoroutinePlaying == false)
                {
                    StartCoroutine(EnemyRam());
                    StartCoroutine(CooldownRoutine(_ramPlayerCoroutine));
                }
            }
        }
    }

    void EnemyFire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(2f, 4f);
            _canFire = Time.time + _fireRate;

            if(_ramPlayerCoroutine == true)
            {
                return;
            }
            else
            {
                LaserFire();
            }
        }
    }

    void LaserFire()
    {
        if(_enemyShootUp == true)
        {
            LasersUp();
        }
        else
        {
            LasersDown();
        }
    }

    void LasersDown()
    {
        GameObject enemyLaser = Instantiate(_enemyLaser, transform.position + _downLaserOffset, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser(true);
        }
    }

    void LasersUp()
    {
        GameObject enemyLaser = Instantiate(_enemyLaser, transform.position + _enemyShootUpOffset, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser(false);
        }
        _enemyShootUp = false;
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 6f, 0f);
        }
    }

    void CollisionExplosion()
    {
        //isAlive = false;
        _audioSource.Play();
        _enemySpeed = 0.25f;
        _animator.SetBool("OnEnemyDeath", true);
        Destroy(gameObject, 2.633f);
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Enemy>());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            CollisionExplosion();
            _player.AddPoints(Random.Range(10, 21));
            Destroy(other.gameObject);
        }

        if (other.tag == "Player")
        {
            CollisionExplosion();
            _player.Damage();
        }

        if (other.tag == "MegaLaser")
        {
            CollisionExplosion();
            _player.AddPoints(Random.Range(10, 21));
        }

        if(other.tag == "Missile")
        {
            CollisionExplosion();
            _player.AddPoints(Random.Range(10, 21));
            Destroy(other.gameObject);
        }
    }

    IEnumerator EnemyRam()
    {
        _enemySpeed = _ramSpeed;
        yield return _ramTime;
        _enemySpeed = _minEnemySpeed;
    }

    IEnumerator CooldownRoutine(bool trigger)
    {
        _isCoroutinePlaying = true;
        trigger = true;
        yield return _cooldownTimer;
        _isCoroutinePlaying = false;
        trigger = false;
    }

    //IEnumerator EnemyLaserRoutine()
    //{
    //    while (isAlive == true)
    //    {
    //        float randomLaser = Random.Range(1f, 3f);
    //        yield return new WaitForSeconds(randomLaser);
    //        GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
    //        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

    //        for (int i = 0; i < lasers.Length; i++)
    //        {
    //            lasers[i].AssignEnemyLaser();
    //        }
    //    }
    //}
}
