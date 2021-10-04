using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool _coroutinePlaying;

    [SerializeField] private GameObject _bossHealthSliderBar;
    [SerializeField] private GameObject _bossShieldSliderBar;

    [SerializeField] private Image _livesIMG;

    [SerializeField] private Sprite[] _spriteLives;

    [SerializeField] private Text _enemiesRemaining;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _ammoDisplay;
    [SerializeField] private Text _outOfAmmo;
    [SerializeField] private Text _engineLasersDisabled;
    [SerializeField] private Text _gameOver;
    [SerializeField] private Text _congratulations;
    [SerializeField] private Text _restartGame;
    [SerializeField] private Text _wave;
    [SerializeField] private Text _bossShieldAndHealth;

    GameManager _gameManager;

    Player _player;

    RectTransform _waveRectTransform;

    WaitForSeconds _waveTextTimer = new WaitForSeconds(2f);
    WaitForSeconds _flickerTimer = new WaitForSeconds(0.5f);


    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _waveRectTransform = transform.Find("Wave_Text").GetComponent<RectTransform>();

        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL in UIManager");
        }

        if(_player = null)
        {
            Debug.LogError("Player is NULL in UIManager");
        }

        if(_waveRectTransform == null)
        {
            Debug.LogError("RectTransform is NULL in UIManager");
        }

        _bossHealthSliderBar.SetActive(false);
        _bossShieldSliderBar.SetActive(false);
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        StartCoroutine(GameOverFlicker());
        _restartGame.gameObject.SetActive(true);
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLivesDisplay(int currentLives)
    {
        // Out of bounds index array erro when the player is on one life and they receive 2 hits from the lasers - NEED TO FIX
        _livesIMG.sprite = _spriteLives[currentLives];

        if(currentLives < 1)
        {
            GameOverSequence();
        }
    }

    public void OutOfAmmo()
    {
        if (_coroutinePlaying == false)
        {
            StartCoroutine(OutOfAmmoRoutine());
        }
        else
        {
            return;
        }
    }

    public void CongraulationsSequence()
    {
        _gameManager.GameOver();
        StartCoroutine(CongraulationsFlicker());
        _restartGame.gameObject.SetActive(true);
    }

    public void UpdateEnemiesRemaining(int currentEnemies)
    {
        _enemiesRemaining.text = currentEnemies.ToString();
    }

    public void UpdateAmmoDisplay(int currentAmmo, int maxAmmo)
    {
        _ammoDisplay.text =  currentAmmo + "/" + maxAmmo;
    }

    public void EngineAndLasersDisabled()
    {
        _engineLasersDisabled.gameObject.SetActive(true);
    }

    public void EngineAndLasersEnabled()
    {
        _engineLasersDisabled.gameObject.SetActive(false);
    }

    public void BossBattleText(string _bossText)
    {
        _bossShieldSliderBar.SetActive(true);
        _bossShieldAndHealth.gameObject.SetActive(true);
        _bossShieldAndHealth.text = _bossText;
    }

    public void BossBattleTextSwitchOff()
    {
        _bossShieldAndHealth.gameObject.SetActive(false);
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOver.gameObject.SetActive(true);
            yield return _flickerTimer;
            _gameOver.gameObject.SetActive(false);
            yield return _flickerTimer;
        }
    }

    IEnumerator CongraulationsFlicker()
    {
        while (true)
        {
            _congratulations.gameObject.SetActive(true);
            yield return _flickerTimer;
            _congratulations.gameObject.SetActive(false);
            yield return _flickerTimer;
        }
    }

    IEnumerator OutOfAmmoRoutine()
    {
        _coroutinePlaying = true;
        _outOfAmmo.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _outOfAmmo.gameObject.SetActive(false);
        _coroutinePlaying = false;
    }

    public IEnumerator StartSliderBars()
    {
        yield return new WaitForSeconds(4f);
        _bossShieldSliderBar.SetActive(true);
        _bossHealthSliderBar.SetActive(true);
    }

    public IEnumerator WaveText(int _waveNumber)
    {
        if(_waveNumber < 6)
        {
            _wave.gameObject.SetActive(true);
            _wave.text = "Wave " + _waveNumber;
            yield return _waveTextTimer;
            _wave.gameObject.SetActive(false);
        }
        else if(_waveNumber == 6)
        {
            _wave.gameObject.SetActive(true);
            _waveRectTransform.localPosition = new Vector3(-400f, 110f, 0f);
            _wave.text = "Boss Wave";
            yield return _waveTextTimer;
            _wave.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Invalid Wave Number");
        }
    }
}
