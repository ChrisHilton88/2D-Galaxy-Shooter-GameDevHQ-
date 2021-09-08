using System.Collections;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private bool _isCoroutineRunning;

    [SerializeField] GameObject _homingMissilePrefab;

    WaitForSeconds _cooldownTimer = new WaitForSeconds(10f);


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isCoroutineRunning == false)
        {
            Instantiate(_homingMissilePrefab, transform.position, Quaternion.identity);
            HomingMissileBehaviour _missileBehaviour = GameObject.Find("Homing Missile(Clone)").GetComponent<HomingMissileBehaviour>();

            if(_missileBehaviour != null)
            {
                _missileBehaviour.FindClosestEnemy();
            }

            StartCoroutine(HomingMissileCooldownRoutine());
        }
        else if (Input.GetKeyDown(KeyCode.E) && _isCoroutineRunning == true)
        {
            Debug.Log("Homing Missile is cooling down");
        }
    }

    IEnumerator HomingMissileCooldownRoutine()
    {
        _isCoroutineRunning = true;
        yield return _cooldownTimer;
        _isCoroutineRunning = false;
    }
}
