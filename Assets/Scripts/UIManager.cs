using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _outOfAmmo;

    [SerializeField] private Image _livesIMG;

    [SerializeField] private Sprite[] _spriteLives;

    [SerializeField] private Text _gameOver;
    [SerializeField] private Text _restartGame;

    private bool _coroutinePlaying = false;

    GameManager _gameManager;
    Player _player;

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

    void GameOverSequence()
    {
        _gameManager.GameOver();
        StartCoroutine(GameOverFlicker());
        _restartGame.gameObject.SetActive(true);
    }

    public void OutOfAmmo()
    {
        if(_coroutinePlaying == false)
        {
            StartCoroutine(OutOfAmmoRoutine());
        }
        else
        {
            return;
        }
    }

    IEnumerator OutOfAmmoRoutine()
    {
        _coroutinePlaying = true;
        _outOfAmmo.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.50f);
        _outOfAmmo.gameObject.SetActive(false);
        _coroutinePlaying = false;
    }
}
