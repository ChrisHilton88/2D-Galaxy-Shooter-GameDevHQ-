using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemySpeed = 3;

    Player player;

    Animator animator;

    AudioSource audioSource;

    [SerializeField] private GameObject _enemyLaser;

    [SerializeField] private float fireRate;
    [SerializeField] private float canFire = -1f;

    //[SerializeField] private bool isAlive;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            Debug.Log("Player script not found in Enemy script");
        }

        if(audioSource == null)
        {
            Debug.LogError("AudioSource not found on Enemy script");
        }

        if(animator == null)
        {
            Debug.LogError("Animator not found on Enemy script");
        }

        //isAlive = true;
       // StartCoroutine(EnemyLaserRoutine());
    }

    void Update()
    {
        CalculateMovement();

        if (Time.time > canFire)
        {
            fireRate = Random.Range(1f, 4f);
            canFire = Time.time + fireRate;
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
            Destroy(GetComponent<Collider2D>());
            //isAlive = false;
            audioSource.Play();
            _enemySpeed = 0.25f;
            animator.SetTrigger("OnEnemyDeath");
            Destroy(other.gameObject);
            player.AddPoints(Random.Range(10, 21));
            Destroy(this.gameObject, 2.633f);
        }

        if(other.tag == "Player")
        {
            Destroy(GetComponent<Collider2D>());
            //isAlive = false;
            audioSource.Play();
            _enemySpeed = 0.25f;
            animator.SetTrigger("OnEnemyDeath");
            player.Damage();
            Destroy(this.gameObject, 2.633f);
        }
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
