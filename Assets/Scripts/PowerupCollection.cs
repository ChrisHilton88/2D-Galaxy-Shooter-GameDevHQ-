using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupCollection : MonoBehaviour
{
    float _moveSpeed = 10f;
    Vector2 _currentPos;
    Vector2 _targetPos;

    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            GameObject[] powerup = GameObject.FindGameObjectsWithTag("Powerup");

            foreach (GameObject powerups in powerup)
            {
                _currentPos = powerups.transform.position;
                Debug.Log(_currentPos);
                _targetPos = transform.position;
                Debug.Log(_targetPos);
                _currentPos = Vector2.Lerp(_currentPos, _targetPos, _moveSpeed);
                Debug.Log(_currentPos);
            }
        }
    }
}
