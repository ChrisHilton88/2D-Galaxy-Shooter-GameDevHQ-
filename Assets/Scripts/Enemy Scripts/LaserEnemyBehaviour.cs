using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemyBehaviour : MonoBehaviour
{
    private float _enemySpeed = 3f;
    private float _fireRate;
    private float _canFire = 2f;
    public float _frequency = 3f;
    public float _magnitude = 1f;
    private float _rightBoundary = 9.5f;

    Vector3 pos;
    Vector3 _spawn;
    Vector3 _enemyMegaLaserOffset = new Vector3(0f, -5f, 0f);

    [SerializeField] private GameObject _enemyMegaLaserPrefab;


    void Start()
    {
        pos = transform.position;

        _spawn = new Vector3(-9.5f, Random.Range(0f, 4f), 0f);

        transform.position = _spawn;
    }

    void Update()
    {
        LaserEnemyMovement();
        LaserEnemyFire();
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

    void LaserEnemyFire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(2f, 4f);
            _canFire = Time.time + _fireRate;
            MegaLaserFire();
        }
    }

    void MegaLaserFire()
    {
        GameObject _enemyMegaLaser = Instantiate(_enemyMegaLaserPrefab, transform.position + _enemyMegaLaserOffset, Quaternion.identity);
        _enemyMegaLaser.transform.parent = transform;
        Destroy(_enemyMegaLaser, 0.5f);
    }

    void Respawn()
    {
        pos = _spawn;
    }
}
