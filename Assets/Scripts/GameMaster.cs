using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public GameObject mousePointer;
    public float levelCountdown;
    public Canvas canvas;
    public Text countdownText;

    private int currentLevel = 0;
    private bool isCounting;


    void Update()
    {

        // Make the mouse marker follow the position of the mouse
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 screenPoint = Input.mousePosition;
            screenPoint.z = 10.0f; //distance of the plane from the camera

            mousePointer.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        }

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
    }

    public void StartLevelCountdown()
    {
        isCounting = true;
    }

    public void StartNewLevel()
    {
        if (currentLevel <= SceneManager.sceneCountInBuildSettings)
        {
            currentLevel++;
            SceneManager.LoadScene(currentLevel);
        }
        else
            Debug.Log("No more levels to load");
    }
}
