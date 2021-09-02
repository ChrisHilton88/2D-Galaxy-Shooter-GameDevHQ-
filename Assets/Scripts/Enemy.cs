using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _enemySpeed = 1;
    private float _fireRate;
    private float _canFire = 2f;
    private float _rayDistance = 6f;

    private bool _rayShot;

    [SerializeField] private GameObject _enemyLaser;

    //[SerializeField] private bool isAlive;

    Player _player;

    Animator _animator;

    AudioSource _audioSource;

    Vector3 _rayOffset = new Vector3(0, -0.4f, 0);


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
        RayCastShot();
    }

    void RayCastShot()
    {
        Debug.DrawRay(transform.position + _rayOffset, (-transform.up * _rayDistance), Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + _rayOffset, -transform.up, _rayDistance);
        if (hit)
        {
            if (hit.collider.tag == "Powerup")
            {
                if(_rayShot == false)
                {
                    LaserFire();
                    StartCoroutine(PowerupShootingCooldownRoutine());
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
            LaserFire();
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
        _animator.SetTrigger("OnEnemyDeath");
        Destroy(this.gameObject, 2.633f);
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Enemy>());
    }

    IEnumerator PowerupShootingCooldownRoutine()
    {
        _rayShot = true;
        yield return new WaitForSeconds(5.0f);
        _rayShot = false;
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
