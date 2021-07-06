using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;

    [SerializeField] private Image _livesIMG;

    [SerializeField] private Sprite[] _spriteLives;

    [SerializeField] private Text gameOver;
    [SerializeField] private Text restartGame;

    private GameManager gameManager;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if(gameManager == null)
        {
            Debug.LogError("GameManager is NULL within UIManager");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLivesDisplay(int currentLives)
    {
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
            gameOver.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            gameOver.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void GameOverSequence()
    {
        gameManager.GameOver();
        StartCoroutine(GameOverFlicker());
        restartGame.gameObject.SetActive(true);
    }
}
