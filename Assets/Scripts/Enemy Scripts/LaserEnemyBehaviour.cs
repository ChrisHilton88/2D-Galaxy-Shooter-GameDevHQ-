using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemyBehaviour : MonoBehaviour
{
    private float _enemySpeed = 3f;
    private float _fireRate;
    private float _canFire = 2f;
    public float _frequency = 3f;
    public float _magnitude = 1f;
    private float _rightBoundary = 9.5f;

    Vector3 pos;
    Vector3 _spawn;
    Vector3 _enemyMegaLaserOffset = new Vector3(0f, -5f, 0f);

    Player _player;

    AudioSource _audioSource;

    Animator _anim;

    SpawnManager _spawnManager;

    [SerializeField] private GameObject _enemyMegaLaserPrefab;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if(_player == null)
        {
            Debug.Log("Player is NULL in LaserEnemyBehaviour");
        }

        if(_audioSource == null)
        {
            Debug.Log("AudioSource is NULL in LaserEnemyBehaviour");
        }

        if(_anim == null)
        {
            Debug.Log("Animator is NULL in LaserEnemyBehaviour");
        }

        if(_spawnManager == null)
        {
            Debug.Log("SpawnManager is NULL in LaserEnemyBehaviour");
        }

        pos = transform.position;
        _spawn = new Vector3(-9.5f, Random.Range(0f, 4f), 0f);
        _canFire = Time.time + 2f;
    }

    void Update()
    {
        LaserEnemyMovement();
        LaserEnemyFire();
    }
    
    void LaserEnemyMovement()
    {
        pos += transform.right * _enemySpeed * Time.deltaTime;
        transform.position = pos + transform.up * Mathf.Sin(Time.time * _frequency) * _magnitude;

        if (transform.position.x > _rightBoundary)
        {
            Respawn();
        }
    }

    void LaserEnemyFire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(2f, 4f);
            _canFire = Time.time + _fireRate;
            MegaLaserFire();
        }
    }

    void MegaLaserFire()
    {
        GameObject _enemyMegaLaser = Instantiate(_enemyMegaLaserPrefab, transform.position + _enemyMegaLaserOffset, Quaternion.identity);
        _enemyMegaLaser.transform.parent = transform;
        Destroy(_enemyMegaLaser, 0.5f);
    }

    void Respawn()
    {
        pos = _spawn;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _player.AddPoints(Random.Range(10, 21));
            Destroy(other.gameObject);
            CollisionExplosion();
        }

        if (other.tag == "Player")
        {
            _player.Damage();
            CollisionExplosion();
        }

        if (other.tag == "MegaLaser")
        {
            CollisionExplosion();
            _player.AddPoints(Random.Range(10, 21));
        }

        if (other.tag == "Missile")
        {
            CollisionExplosion();
            _player.AddPoints(Random.Range(10, 21));
            Destroy(other.gameObject);
        }
    }

    void CollisionExplosion()
    {
        _audioSource.Play();
        _enemySpeed = 0.25f;
        _anim.SetBool("OnEnemyDeath", true);
        Destroy(gameObject, 2.633f);
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<LaserEnemyBehaviour>());
    }
}
