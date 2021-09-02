using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileBehaviour : MonoBehaviour
{
    private float _missileSpeed = 5f;

    private bool _enemyTargetFound;

    Enemy _closestEnemy;


    void Update()
    {
        if (_enemyTargetFound == true)
        {
            HomingMissileMove();
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
        Enemy[] _enemyList = FindObjectsOfType<Enemy>();

        if (_enemyList != null)
        {
            _enemyTargetFound = true;

            foreach (Enemy enemy in _enemyList)
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

    void HomingMissileMove()
    {
        Vector3 direction = transform.position - _closestEnemy.transform.position;
        direction = -direction.normalized;
        transform.rotation = Quaternion.LookRotation(transform.forward, direction);
        transform.position += direction * _missileSpeed * Time.deltaTime;
    }
}
