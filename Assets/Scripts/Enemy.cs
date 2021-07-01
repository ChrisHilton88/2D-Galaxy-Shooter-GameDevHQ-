using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemySpeed = 5;

    Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        if (player == null)
        {
            Debug.Log("Player script not found in Enemy script");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * enemySpeed * Time.deltaTime);

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
            Debug.Log("Hit: " + other.transform.name);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

        if(other.tag == "Player")
        {
            Debug.Log("Hit: " + other.transform.name);
            player.Damage();
            Destroy(this.gameObject);
        }
    }
}
