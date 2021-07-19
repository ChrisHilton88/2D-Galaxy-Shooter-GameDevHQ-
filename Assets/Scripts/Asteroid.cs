using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private int rotateSpeed = 20;

    SpawnManager spawnManager;

    [SerializeField] private GameObject _explosionPrefab;

    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        if(spawnManager == null)
        {
            Debug.LogError("SpawnManager is Null : Asteroid");
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, 1 * rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            spawnManager.StartSpawning();
            Destroy(this.gameObject);
        }
    }
}
