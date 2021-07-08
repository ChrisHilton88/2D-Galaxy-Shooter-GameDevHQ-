using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemySpeed = 5;
    [SerializeField] private float _slowRate = 2;

    Player player;

    Animator animator;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        animator = GetComponent<Animator>();

        if (player == null)
        {
            Debug.Log("Player script not found in Enemy script");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if(transform.position.y < -5)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 6, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _enemySpeed = 0.25f;
            animator.SetTrigger("OnEnemyDeath");
            Destroy(other.gameObject);
            player.AddPoints(Random.Range(10, 21));
            Destroy(this.gameObject, 2.633f);
        }

        if(other.tag == "Player")
        {
            _enemySpeed = 0;
            animator.SetTrigger("OnEnemyDeath");
            player.Damage();
            Destroy(this.gameObject);
        }
    }
}
