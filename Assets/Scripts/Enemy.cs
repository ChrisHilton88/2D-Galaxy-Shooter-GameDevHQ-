using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _enemySpeed = 3;
    private float _fireRate;
    private float _canFire = -1f;

    [SerializeField] private GameObject _enemyLaser;

    //[SerializeField] private bool isAlive;

    Player _player;

    Animator _animator;

    AudioSource _audioSource;


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

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(1f, 4f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
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

        if(other.tag == "Player")
        {
            CollisionExplosion();
            _player.Damage();

        }

        if (other.tag == "MegaLaser")
        {
            CollisionExplosion();
            _player.AddPoints(Random.Range(10, 21));
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
