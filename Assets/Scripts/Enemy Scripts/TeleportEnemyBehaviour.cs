using System.Collections;
using UnityEngine;

public class TeleportEnemyBehaviour : MonoBehaviour
{
    private float _enemySpeed = 5;
    private float _avoidShotDistance = 3f;
    private float _shieldHits = 1;

    private bool _isShieldEnabled;
    private bool _isAvoidShotCoroutineEnabled;

    Vector3 _endPos;
    Vector3 _rayOffset = new Vector3(0, -0.6f, 0);

    AudioSource _audioSource;

    Animator _anim;

    Player _player;

    SpriteRenderer _shieldSpriteRend;

    SpawnManager _spawnManager;

    WaitForSeconds _cooldown = new WaitForSeconds(3.0f);

    [SerializeField] private GameObject _enemyShield;


    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _shieldSpriteRend = transform.Find("Enemy_Shield").GetComponentInChildren<SpriteRenderer>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if (_audioSource == null)
        {
            Debug.Log("Audio Source is NULL in Teleport Enemy");
        }

        if (_anim == null)
        {
            Debug.Log("Animator is NULL in Teleport Enemy");
        }

        if (_player == null)
        {
            Debug.Log("Player is NULL in Teleport Enemy");
        }

        if (_shieldSpriteRend == null)
        {
            Debug.Log("Shield is null in Teleport Enemy");
        }

        if (_spawnManager == null)
        {
            Debug.Log("SpawnManager is NULL in LaserEnemyBehaviour");
        }

        _isShieldEnabled = true;
        _enemyShield.SetActive(true);
        _endPos = new Vector3(Random.Range(-8f, 8f), -7f, 0f);
    }

    void Update()
    {
        CalculateMovement();
        AvoidShot();
    }

    void CalculateMovement()
    {
        Vector3 dir = transform.position - _endPos;
        dir = -dir.normalized;
        transform.position += dir * _enemySpeed * Time.deltaTime;

        if (transform.position.y < -6f)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        transform.position = new Vector3(Random.Range(-10f, 10f), 6f, 0f);
        _endPos = new Vector3(Random.Range(-8f, 8f), -7f, 0f);
    }

    void AvoidShot()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + _rayOffset, -transform.up, _avoidShotDistance);
        if (hit)
        {
            if (hit.collider.tag == "Laser")
            {
                if (_isAvoidShotCoroutineEnabled == false)
                {
                    Respawn();
                    StartCoroutine(AvoidShotCooldownRoutine());
                }
            }
        }
    }

    void ShieldDamage()
    {
        if(_isShieldEnabled == true)
        {
            _shieldHits--;
            _audioSource.Play();

            if(_shieldHits <= 0)
            {
                _isShieldEnabled = false;
                _enemyShield.SetActive(false);
            }
        }
        else
        {
            CollisionExplosion();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            ShieldDamage();
            _player.AddPoints(Random.Range(10, 21));
            Destroy(other.gameObject);
        }

        if (other.tag == "Player")
        {
            ShieldDamage();
            _player.Damage();
        }

        if (other.tag == "MegaLaser")
        {
            _player.AddPoints(Random.Range(10, 21));
            CollisionExplosion();
            Destroy(GetComponentInChildren<SpriteRenderer>());
        }

        if (other.tag == "Missile")
        {
            _player.AddPoints(Random.Range(10, 21));
            CollisionExplosion();
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
        Destroy(GetComponent<Enemy>());
    }

    IEnumerator AvoidShotCooldownRoutine()
    {
        _isAvoidShotCoroutineEnabled = true;
        yield return _cooldown;
        _isAvoidShotCoroutineEnabled = false;
    }
}
