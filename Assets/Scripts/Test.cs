using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    private bool _isShieldEnabled;

    SpriteRenderer _shieldSpriteRend;

    [SerializeField] private GameObject _enemyShield;


    // Start is called before the first frame update
    void Start()
    {
        _shieldSpriteRend = transform.Find("Enemy_Shield").GetComponentInChildren<SpriteRenderer>();

        if (_shieldSpriteRend == null)
        {
            Debug.Log("Shield is null in Teleport Enemy");
        }

        _enemyShield.SetActive(false);
    }
}
