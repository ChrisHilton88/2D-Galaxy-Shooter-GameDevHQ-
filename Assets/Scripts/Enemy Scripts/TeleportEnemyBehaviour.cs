using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnemyBehaviour : MonoBehaviour
{
    private float _enemySpeed = 5;
    private float _avoidShotDistance = 3f;

    private bool _isAvoidShotCoroutineEnabled;

    Vector3 _endPos;
    Vector3 _rayOffset = new Vector3(0, -0.6f, 0);

    AudioSource _audioSource;

    Animator _anim;

    Player _player;

    WaitForSeconds _cooldown = new WaitForSeconds(3.0f);

    // In SpawnManager, when this enemy is instantiated, create a random event that will determine whether the enemy shield will be activated
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();

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

        transform.position = new Vector3(Random.Range(-8f, 8f), 6f, 0f);
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
        Debug.DrawRay(transform.position + _rayOffset, (-transform.up * _avoidShotDistance), Color.red);
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
        Destroy(GetComponent<Enemy>());
    }

    IEnumerator AvoidShotCooldownRoutine()
    {
        _isAvoidShotCoroutineEnabled = true;
        yield return _cooldown;
        _isAvoidShotCoroutineEnabled = false;
    }
}
