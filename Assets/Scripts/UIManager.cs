using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;

    [SerializeField] private Image _livesIMG;

    [SerializeField] private Sprite[] _spriteLives;

    [SerializeField] private GameObject gameOver;


    void Start()
    {
        _scoreText.text = "Score: " + 0;
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
            StartCoroutine(GameOverFlicker());
        }
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            gameOver.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            gameOver.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
