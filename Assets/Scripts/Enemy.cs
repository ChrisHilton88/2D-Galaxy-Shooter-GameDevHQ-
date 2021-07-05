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
            Destroy(other.gameObject);
            player.AddPoints(Random.Range(10, 21));
            // As the max range is not inclusive in Random.Range while using Int, increase by 1 
            Destroy(this.gameObject);
        }

        if(other.tag == "Player")
        {
            player.Damage();
            Destroy(this.gameObject);
        }
    }
}
