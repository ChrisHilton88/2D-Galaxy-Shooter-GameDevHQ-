using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{ 
    [SerializeField] private int powerupSpeed = 3;

    Player player;

    [SerializeField] private int powerupID;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        if(player == null)
        {
            Debug.LogError("Player script not found within Powerups script");
        }
    }


    void Update()
    {
        transform.Translate(Vector3.down * powerupSpeed * Time.deltaTime);

        if(transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(powerupID == 0)
            {
                Debug.Log("Collected Triple Shot Powerup");
                player.TripleShotActive();
                Destroy(this.gameObject);
            }
            else if(powerupID == 1)
            {
                Debug.Log("Collected Speed Boost Powerup");
                player.SpeedBoostActive();
                Destroy(this.gameObject);
            }
            else if(powerupID == 2)
            {
                Debug.Log("Collected Shields Powerup");
                // Shields
                Destroy(this.gameObject);
            }
        }
    }
}
