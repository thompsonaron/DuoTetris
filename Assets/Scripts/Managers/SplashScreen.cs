using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public GameObject howToPlayCanvas;
    public GameObject highScoreCanvas;
    public Text highscoreText;
    public HighScore highScore;

    public void ExitApplication()
    {
        Application.Quit();
    }
    public void StartTetris()
    {
        SceneManager.LoadScene(1);
    }
    public void ToggleHowToPlay()
    {
        howToPlayCanvas.gameObject.SetActive(!howToPlayCanvas.gameObject.activeSelf);
    }

    public void ToggleHighScore()
    {
        highScoreCanvas.gameObject.SetActive(!highScoreCanvas.gameObject.activeSelf);
        highscoreText.text = highScore.LoadData();
    }
}
