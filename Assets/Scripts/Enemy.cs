using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemySpeed = 5;


    void Update()
    {
        transform.Translate(Vector3.down * enemySpeed * Time.deltaTime);

        if(transform.position.y < -5)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 6, 0);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Enter code
    }
}

//private void OnTriggerEnter2D(Collider2D collision)
//{
//    if (collision.tag == "Laser" || collision.tag == "Player")
//    {
//        Debug.Log("Hit: " + collision.transform.name);
//        Destroy(collision.gameObject);
//        Destroy(this.gameObject);
//    }
//}
