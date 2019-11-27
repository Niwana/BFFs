using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public GameObject mousePointer;
    public float levelCountdown;
    public Canvas canvas;
    public Text countdownText;
    public Image tutorialImage;
    public bool shownTutorial;

    private bool isCounting;

    private void Start()
    {

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                tutorialImage.gameObject.SetActive(true);
            }

            countdownText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Make it possible to restart the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Loads the next level
        if (isCounting && levelCountdown > 0)
        {
            levelCountdown -= 1 * Time.deltaTime;
            canvas.gameObject.SetActive(true);
            countdownText.text = (levelCountdown + 1).ToString("F0");

            if (levelCountdown <= 0)
            {
                isCounting = false;
                StartNewLevel();
            }
        }

        if (Input.anyKeyDown && !shownTutorial && SceneManager.GetActiveScene().buildIndex != 0)
        {
            tutorialImage.gameObject.SetActive(false);
            shownTutorial = true;
        }
    }

    public void StartLevelCountdown()
    {
        countdownText.gameObject.SetActive(true);
        isCounting = true;
    }

    public void StartNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
