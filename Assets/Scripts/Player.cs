using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 12.5f;

    public float xBoundary = 8.5f, yBoundary = 4.5f;

    private Vector3 laserOffset = new Vector3(0, 1, 0);

    public GameObject laserPrefab;

    public float canFire = 0;
    public float fireRate = 0.25f;



    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
        {
            FireLaser();
        }
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * speed * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xBoundary, xBoundary), Mathf.Clamp(transform.position.y, -yBoundary, yBoundary), 0);
    }

    void FireLaser()
    {
        canFire = Time.time + fireRate;
        Instantiate(laserPrefab, transform.position + laserOffset, Quaternion.identity);
    }
}
