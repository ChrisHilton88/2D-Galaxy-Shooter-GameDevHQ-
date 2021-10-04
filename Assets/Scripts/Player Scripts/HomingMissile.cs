using System.Collections;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private int _waveNumber;

    private bool _isCoroutineRunning;

    [SerializeField] private GameObject _homingMissilePrefab;

    WaitForSeconds _cooldownTimer = new WaitForSeconds(10f);


    void Start()
    {
        _waveNumber = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isCoroutineRunning == false)
        {
            Instantiate(_homingMissilePrefab, transform.position, Quaternion.identity);
            HomingMissileBehaviour _missileBehaviour = GameObject.Find("Homing Missile(Clone)").GetComponent<HomingMissileBehaviour>();

            if(_missileBehaviour != null)
            {
                _missileBehaviour.FindClosestEnemy(_waveNumber);
            }

            StartCoroutine(HomingMissileCooldownRoutine());
        }
        else if (Input.GetKeyDown(KeyCode.E) && _isCoroutineRunning == true)
        {
            Debug.Log("Homing Missile is cooling down");
        }
    }

    public void IncreaseWaveNumber()
    {
        _waveNumber++;
    }

    IEnumerator HomingMissileCooldownRoutine()
    {
        _isCoroutineRunning = true;
        yield return _cooldownTimer;
        _isCoroutineRunning = false;
    }
}
