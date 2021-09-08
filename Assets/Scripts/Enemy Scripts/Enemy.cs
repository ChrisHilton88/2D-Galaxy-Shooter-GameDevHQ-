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
    [SerializeField] private bool _ramPlayerCoroutine;

    [SerializeField] private GameObject _enemyLaser;

    //[SerializeField] private bool isAlive;

    Player _player;

    Animator _animator;

    AudioSource _audioSource;

    WaitForSeconds _ramTime = new WaitForSeconds(0.5f);

    Vector3 _rayOffset = new Vector3(0, -0.6f, 0);


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();

        if (_player == null)
        {
            Debug.Log("Player script not found in Enemy script");
        }

        if(_audioSource == null)
        {
            Debug.LogError("AudioSource not found on Enemy script");
        }

        if(_animator == null)
        {
            Debug.LogError("Animator not found on Enemy script");
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
    }

    void RayCastPowerupShoot()
    {
        Debug.DrawRay(transform.position + _rayOffset, (-transform.up * _rayPowerupDistance), Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + _rayOffset, -transform.up, _rayPowerupDistance);
        if (hit)
        {
            if (hit.collider.tag == "Powerup")
            {
                if (_powerupShotTrigger == false)
                {
                    LaserFire();
                    StartCoroutine(CooldownRoutine(5.0f, _powerupShotTrigger, "Started Powerup Shot Cooldown", "Finished Powerup Shot Cooldown"));
                }
            }
        }
    }

    void RayCastRamPlayer()
    {
        Debug.DrawRay(transform.position + _rayOffset, (-transform.up * _ramRayDistance), Color.yellow);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + _rayOffset, -transform.up, _ramRayDistance);
        if (hit)
        {
            if(hit.collider.tag == "Player")
            {
                if(_ramPlayerCoroutine == false)
                {
                    StartCoroutine(EnemyRam());
                    StartCoroutine(CooldownRoutine(5f, _ramPlayerCoroutine, "Started Ram Cooldown", "Finished Ram Cooldown"));
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
        GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -5)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 6, 0);
        }
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

    IEnumerator EnemyRam()
    {
        _enemySpeed = _ramSpeed;
        yield return _ramTime;
        _enemySpeed = _minEnemySpeed;
    }

    IEnumerator CooldownRoutine(float time, bool trigger, string firstMessage, string secondMessage)
    {
        trigger = true;
        yield return new WaitForSeconds(time);
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
