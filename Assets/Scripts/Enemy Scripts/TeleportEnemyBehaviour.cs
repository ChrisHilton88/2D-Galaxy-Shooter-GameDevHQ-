using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnemyBehaviour : MonoBehaviour
{
    private float _enemySpeed = 5;


    Vector3 _endPos;




    // In SpawnManager, when this enemy is instantiated, create a random event that will determine whether the enemy shield will be activated

    void Start()
    {
        transform.position = new Vector3(Random.Range(-8f, 8f), 6f, 0f);
        _endPos = new Vector3(Random.Range(-8f, 8f), -7f, 0f);
    }

    void Update()
    {
        CalculateMovement();
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

}
