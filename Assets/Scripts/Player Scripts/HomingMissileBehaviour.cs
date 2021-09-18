using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileBehaviour : MonoBehaviour
{
    private float _missileSpeed = 5f;

    private bool _enemyTargetFound;

    Vector3 _playClipAtPoint = new Vector3(0, 0, 0);

    GameObject _closestEnemy;

    Animator _anim;

    [SerializeField] AudioClip _explosionClip;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (_enemyTargetFound == true)
        {
            HomingMissileMovement();
        }
        else
        {
            transform.position += Vector3.up * _missileSpeed * Time.deltaTime;

            if(transform.position.y > 6f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void FindClosestEnemy()
    {
        float _closestDistanceToEnemy = Mathf.Infinity;
        GameObject[] _enemyList = GameObject.FindGameObjectsWithTag("Enemy");

        if (_enemyList != null)
        {
            _enemyTargetFound = true;

            foreach (GameObject enemy in _enemyList)
            {
                float _distanceToEnemy = (enemy.transform.position - transform.position).sqrMagnitude;

                if (_distanceToEnemy < _closestDistanceToEnemy)
                {
                    _closestDistanceToEnemy = _distanceToEnemy;
                    _closestEnemy = enemy;
                }
            }
        }

        if (_enemyList.Length == 0)
        {
            _enemyTargetFound = false;
            Debug.Log("No enemies found!");
        }
    }

    void HomingMissileMovement()
    {
        Vector3 direction = transform.position - _closestEnemy.transform.position;
        direction = -direction.normalized;
        transform.rotation = Quaternion.LookRotation(transform.forward, direction);
        transform.position += direction * _missileSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _anim.SetBool("OnLaserCollision", true); 
            AudioSource.PlayClipAtPoint(_explosionClip, _playClipAtPoint, 1f);
            Destroy(gameObject, 2.633f);
            Destroy(other.gameObject);
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<HomingMissileBehaviour>());
        }
    }
}
