﻿using System.Collections;
using UnityEngine;

public class Powerups : MonoBehaviour
{ 
    private int _powerupSpeed = 3;

    // Added
    private bool _isMegeLaserEnabled = false;

    Player player;

    [SerializeField] private int _powerupID;

    [SerializeField] private AudioClip _clip; 

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
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);

        if (transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_clip, transform.position, 1f);

            switch (_powerupID)
            {
                case 0:
                    player.TripleShotActive();
                    Destroy(gameObject);
                    break;
                case 1:
                    player.SpeedBoostActive();
                    Destroy(gameObject);
                    break;
                case 2:
                    player.ShieldBoostActive();
                    Destroy(gameObject);
                    break;
                case 3:
                    player.AmmoRefillActive();
                    Destroy(gameObject);
                    break;
                case 4:
                    player.HealthRefillActive();
                    Destroy(gameObject);
                    break;
                    // Added
                case 5:
                    player.MegaLaserActive();
                    Destroy(gameObject);
                    break;
                default:
                    Debug.Log("Unknown value");
                    break;
            }
        }
    }
}
