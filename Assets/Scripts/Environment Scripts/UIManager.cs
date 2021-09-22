using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool _coroutinePlaying = false;

    [SerializeField] private Text _enemiesRemaining;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _ammoDisplay;
    [SerializeField] private Text _outOfAmmo;
    [SerializeField] private Text _engineLasersDisabled;

    [SerializeField] private Image _livesIMG;

    [SerializeField] private Sprite[] _spriteLives;

    [SerializeField] private Text _gameOver;
    [SerializeField] private Text _restartGame;
    [SerializeField] private Text _wave;

    GameManager _gameManager;
    Player _player;

    WaitForSeconds _waveTextTimer = new WaitForSeconds(2f);

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL within UIManager");
        }

        if(_player = null)
        {
            Debug.LogError("Player is null : UIManager");
        }
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

    void GameOverSequence()
    {
        _gameManager.GameOver();
        StartCoroutine(GameOverFlicker());
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

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOver.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOver.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
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

    public IEnumerator WaveText(int _waveNumber)
    {
        _wave.gameObject.SetActive(true);
        _wave.text = "Wave " + _waveNumber;
        yield return _waveTextTimer;
        _wave.gameObject.SetActive(false);
    }
}
