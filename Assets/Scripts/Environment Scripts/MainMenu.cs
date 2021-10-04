using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image _backgroundInstructionsImage;
    [SerializeField] private Image _backgroundWeaponsImage;
    [SerializeField] private Image _backgroundPowerupsImage;
    [SerializeField] private Image _backgroundEnemiesImage;

    [SerializeField] private Text _helloAndWelcomeText;
    [SerializeField] private Text _instructionsHeadingText;
    [SerializeField] private Text _weaponsHeadingText;
    [SerializeField] private Text _powerupsHeadingText;
    [SerializeField] private Text _enemiesHeadingText;


    void Start()
    {
        _backgroundInstructionsImage.gameObject.SetActive(false);
        _backgroundWeaponsImage.gameObject.SetActive(false);
        _backgroundPowerupsImage.gameObject.SetActive(false);
        _backgroundEnemiesImage.gameObject.SetActive(false);
        _helloAndWelcomeText.gameObject.SetActive(false);
        _instructionsHeadingText.gameObject.SetActive(false);
        _powerupsHeadingText.gameObject.SetActive(false);
        _weaponsHeadingText.gameObject.SetActive(false);
        _enemiesHeadingText.gameObject.SetActive(false);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadInstructionsUI()
    {
        _backgroundInstructionsImage.gameObject.SetActive(true);
        _helloAndWelcomeText.gameObject.SetActive(true);
        _instructionsHeadingText.gameObject.SetActive(true);
    }

    public void InstructionsBackUI()
    {
        _backgroundInstructionsImage.gameObject.SetActive(false);
        _helloAndWelcomeText.gameObject.SetActive(false);
        _instructionsHeadingText.gameObject.SetActive(false);
    }

    public void LoadWeaponsUI()
    {
        _backgroundWeaponsImage.gameObject.SetActive(true);
        _weaponsHeadingText.gameObject.SetActive(true);
    }

    public void WeaponsBackUI()
    {
        _backgroundWeaponsImage.gameObject.SetActive(false);
        _weaponsHeadingText.gameObject.SetActive(false);
    }

    public void LoadPowerupsUI()
    {
        _backgroundPowerupsImage.gameObject.SetActive(true);
        _powerupsHeadingText.gameObject.SetActive(true);
    }

    public void PowerupsBackUI()
    {
        _backgroundPowerupsImage.gameObject.SetActive(false);
        _powerupsHeadingText.gameObject.SetActive(false);
    }

    public void LoadEnemiesUI()
    {
        _backgroundEnemiesImage.gameObject.SetActive(true);
        _enemiesHeadingText.gameObject.SetActive(true);
    }

    public void EnemiesBackUI()
    {
        _backgroundEnemiesImage.gameObject.SetActive(false);
        _enemiesHeadingText.gameObject.SetActive(false);
    }
}
