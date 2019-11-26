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

    private int currentLevel;
    private bool isCounting;

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("shownTutorial", 0);
    }

    private void Start()
    {


        if (PlayerPrefs.GetInt("shownTutorial") == 0)
        {
            tutorialImage.gameObject.SetActive(true);
        }

        countdownText.gameObject.SetActive(false);
    }

    void Update()
    {

        // Make the mouse marker follow the position of the mouse
        /*
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = 10.0f; //distance of the plane from the camera

            mousePointer.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        }
        */

        // Make it possible to restart the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("currentLevel"));
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

        if (Input.anyKeyDown && !shownTutorial)
        {
            tutorialImage.gameObject.SetActive(false);
            //shownTutorial = true;
            PlayerPrefs.SetInt("shownTutorial", 1);
        }
    }

    public void StartLevelCountdown()
    {
        countdownText.gameObject.SetActive(true);
        isCounting = true;
    }

    public void StartNewLevel()
    {
        if (PlayerPrefs.GetInt("currentLevel") < SceneManager.sceneCountInBuildSettings - 1)
        {
            currentLevel = PlayerPrefs.GetInt("currentLevel");
            currentLevel++;
            PlayerPrefs.SetInt("currentLevel", currentLevel);
            SceneManager.LoadScene(currentLevel);
            Debug.Log("Current: " + currentLevel);
        }
        else
        {
            Debug.Log("No more levels to load");
            SceneManager.LoadScene(PlayerPrefs.GetInt("currentLevel"));
        }
    }
}
