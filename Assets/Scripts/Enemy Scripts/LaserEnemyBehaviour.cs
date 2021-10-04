using UnityEngine;

public class LaserEnemyBehaviour : MonoBehaviour
{
    private float _enemySpeed = 3f;
    private float _fireRate;
    private float _canFire = 2f;
    private float _frequency = 4f;
    private float _magnitude = 1f;
    private float _rightBoundary = 9.5f;
    private float _random;

    Vector3 _thisPos;
    Vector3 _spawn;
    Vector3 _enemyMegaLaserOffset = new Vector3(0f, -5f, 0f);

    [SerializeField] private GameObject _enemyMegaLaserPrefab;

    Animator _anim;

    AudioSource _audioSource;

    Player _player;

    SpawnManager _spawnManager;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if(_player == null)
        {
            Debug.LogError("Player is NULL in LaserEnemyBehaviour");
        }

        if(_audioSource == null)
        {
            Debug.LogError("AudioSource is NULL in LaserEnemyBehaviour");
        }

        if(_anim == null)
        {
            Debug.LogError("Animator is NULL in LaserEnemyBehaviour");
        }

        if(_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL in LaserEnemyBehaviour");
        }

        _thisPos = transform.position;
        _spawn = new Vector3(-9.5f, Random.Range(0f, 4f), 0f);
        _random = Random.Range(2f, 4f);
        _canFire = Time.time + _random;
    }

    void Update()
    {
        LaserEnemyMovement();
        LaserEnemyFire();
    }
    
    void LaserEnemyMovement()
    {
        _thisPos += transform.right * _enemySpeed * Time.deltaTime;
        transform.position = _thisPos + transform.up * Mathf.Sin(Time.time * _frequency) * _magnitude;

        if (transform.position.x > _rightBoundary)
        {
            Respawn();
        }
    }

    void LaserEnemyFire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(1f, 3f);
            _canFire = Time.time + _fireRate;
            MegaLaserFire();
        }
    }

    void MegaLaserFire()
    {
        GameObject _enemyMegaLaser = Instantiate(_enemyMegaLaserPrefab, transform.position + _enemyMegaLaserOffset, Quaternion.identity);
        _enemyMegaLaser.transform.parent = transform;
        Destroy(_enemyMegaLaser, 0.25f);
    }

    void Respawn()
    {
        _thisPos = _spawn;
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
}
