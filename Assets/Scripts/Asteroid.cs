using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private int rotateSpeed = 10;

    Animator anim;

    SpawnManager spawnManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        transform.Rotate(0, 0, 1 * rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            anim.SetTrigger("OnAsteroidExplosion");
            spawnManager.StartSpawning();
            Destroy(this.gameObject, 2.633f);
        }
    }
}
