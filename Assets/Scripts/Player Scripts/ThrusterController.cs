using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterController : MonoBehaviour
{
    private float _maxThrusterPercent = 100f;
    private float _minThrusterPercent = 0f;
    private float _thrustDecrease = 1f;
    private float _thrustIncrease = 0.5f;
    private float _thrusterPercent = 100f;

    private bool _isOverloaded;
    private bool _coroutinePlaying;
    private bool _isNegativePickupEnabled;

    Color _thrustGaugeOverload = new Color(1, 0, 0, 1);
    Color _thrustGaugeNormal = new Color(0, 0, 1, 1);
    Color _backgroundFillWhite = new Color(1, 1, 1, 1);

    [SerializeField] AudioClip _rechargeThrusterClip;
    [SerializeField] AudioClip _overloadAlarmClip;
    [SerializeField] AudioClip _thrusterBoosterClip;

    AudioSource _audioSource;

    Image _fillImage;
    Image _backgroundFillImage;

    Player _player;

    Slider _thrusterBar;


    void Start()
    {
        _backgroundFillImage = GameObject.Find("Background").GetComponent<Image>();
        _fillImage = GameObject.Find("Fill").GetComponent<Image>();
        _thrusterBar = GameObject.Find("Thruster Bar").GetComponent<Slider>();
        _player = GameObject.Find("Player").GetComponentInParent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if(_backgroundFillImage == null)
        {
            Debug.LogError("BackgroundImage is NULL in ThrusterController");
        }

        if(_fillImage == null)
        {
            Debug.LogError("FillImage is NULL in ThrusterController");
        }

        if(_thrusterBar == null)
        {
            Debug.LogError("ThrusterBar is NULL in ThrusterController");
        }

        if (_player == null)
        {
            Debug.LogError("Player is NULL in ThrusterController");
        }

        if(_audioSource == null)
        {
            Debug.LogError("AudioSource is NULL in ThrusterController");
        }

        _fillImage.color = _thrustGaugeNormal;
    }

    void Update()
    {
        _thrusterBar.value = _thrusterPercent;
        ThrusterBoost();
        PlayAudio();
    }

    void ThrusterBoost()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _thrusterPercent > _minThrusterPercent)
        {
            _player.ThrusterBoostActivated();
            _thrusterPercent -= _thrustDecrease;

            if (_thrusterPercent <= _minThrusterPercent)
            {
                EngineOverload();
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift) && _thrusterPercent == _minThrusterPercent)
        {
            if (_coroutinePlaying == true)
            {
                return;
            }
        }
        else
        {
            if(_isNegativePickupEnabled == false)
            {
                _player.ThrusterBoostDeactivated();

                if (_thrusterPercent >= _maxThrusterPercent)
                {
                    _thrusterPercent = _maxThrusterPercent;
                    _audioSource.Stop();
                }

                if (_isOverloaded == false && _thrusterPercent < _maxThrusterPercent)
                {
                    _thrusterPercent += _thrustIncrease;
                }
            }
            else
            {
                return;
            }
        }
    }

    void EngineOverload()
    {
        _thrusterPercent = _minThrusterPercent;
        _isOverloaded = true;
        _player.ThrusterOverload();
        StartCoroutine(ThrusterOverloadRoutine());
    }

    void PlayAudio()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _isOverloaded == false)
        {
            PlayThrusterBoostClip();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && _isOverloaded == false)
        {
            PlayThrusterRechargeClip();
        }
    }

    void PlayThrusterBoostClip()
    {
        _audioSource.clip = _thrusterBoosterClip;
        _audioSource.volume = 1f;
        _audioSource.Play();
    }

    void PlayOverloadAlarmClip()
    {
        _audioSource.clip = _overloadAlarmClip;
        _audioSource.volume = 0.1f;
        _audioSource.Play();
    }

    void PlayThrusterRechargeClip()
    {
        _audioSource.clip = _rechargeThrusterClip;
        _audioSource.volume = 0.5f;
        _audioSource.Play();
    }

    IEnumerator ThrusterOverloadRoutine()
    {
        while(_isOverloaded == true)
        {
            _coroutinePlaying = true;
            PlayOverloadAlarmClip();
            StartCoroutine(ThrusterGaugeFlicker());
            yield return new WaitForSeconds(3.0f);
            PlayThrusterRechargeClip();
            _backgroundFillImage.color = _backgroundFillWhite;
            _fillImage.color = _thrustGaugeNormal;
            _isOverloaded = false;
            _coroutinePlaying = false;
        }
    }

    IEnumerator ThrusterGaugeFlicker()
    {
        while(_coroutinePlaying == true)
        {
            _backgroundFillImage.color = _thrustGaugeOverload;
            _fillImage.color = _thrustGaugeOverload;
            yield return new WaitForSeconds(0.25f);
            _backgroundFillImage.color = _backgroundFillWhite;
            _fillImage.color = _backgroundFillWhite;
            yield return new WaitForSeconds(0.25f);
        }
    }

    public IEnumerator NegativePickupRoutine()
    {
        _isNegativePickupEnabled = true;
        yield return new WaitForSeconds(4f);
        _isNegativePickupEnabled = false;
    }
}



