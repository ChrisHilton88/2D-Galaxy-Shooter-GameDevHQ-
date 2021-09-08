using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemyBehaviour : MonoBehaviour
{
    private float _enemySpeed = 3f;
    public float _frequency = 3f;
    public float _magnitude = 1f;
    private float _rightBoundary = 9.5f;

    Vector3 pos;

    Vector3 _spawn;


    void Start()
    {
        pos = transform.position;

        _spawn = new Vector3(-9.5f, Random.Range(0f, 4f), 0f);

        transform.position = _spawn;
    }

    void Update()
    {
        LaserEnemyMovement();
    }
    
    void LaserEnemyMovement()
    {
        pos += transform.right * _enemySpeed * Time.deltaTime;
        transform.position = pos + transform.up * Mathf.Sin(Time.time * _frequency) * _magnitude;

        if (transform.position.x > _rightBoundary)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        pos = _spawn;
    }
}
