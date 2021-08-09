using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterController : MonoBehaviour
{
    //Slider thrusterBar;

    Player _player;

    private float _maxThrusterPercent = 100f;
    private float _minThrusterPercent = 0f;
    private float _thrustDecrease = 0.5f;
    private float _thrustIncrease = 0.25f;
    [SerializeField] private float _thrusterPercent = 100f;

    private bool _isOverloaded = false;
    private bool _coroutinePlaying = false;

    void Start()
    {
        //thrusterBar = GetComponent<Slider>();
        _player = GameObject.Find("Player").GetComponentInParent<Player>();

        if (_player == null)
        {
            Debug.Log("Player script not found within ThrusterBarController script");
        }
    }

    void Update()
    {
        //thrusterBar.value = _thrusterPercent;
        ThrusterBoost();
    }

    void ThrusterBoost()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _thrusterPercent > _minThrusterPercent)
        {
            _player.ThrusterBoostActivated();
            _thrusterPercent -= _thrustDecrease;

            if(_thrusterPercent <= _minThrusterPercent)
            {
                _thrusterPercent = _minThrusterPercent;
                _isOverloaded = true;
                _player.ThrusterOverload();
                StartCoroutine(ThrusterOverloadRoutine());
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift) && _thrusterPercent == _minThrusterPercent)
        {
            if(_coroutinePlaying == true)
            {
                return;
            }
        }

        else
        {
            _player.ThrusterBoostDeactivated();

            if (_thrusterPercent >= _maxThrusterPercent)
            {
                _thrusterPercent = _maxThrusterPercent;
            }

            if (_isOverloaded == false)
            {
                _thrusterPercent += _thrustIncrease;
            }
        }

    }

    IEnumerator ThrusterOverloadRoutine()
    {
        while(_isOverloaded == true)
        {
            _coroutinePlaying = true;
            yield return new WaitForSeconds(3.0f);
            _isOverloaded = false;
            _coroutinePlaying = false;
        }
    }
}



