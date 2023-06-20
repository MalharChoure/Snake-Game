using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settingsscript : MonoBehaviour
{
    public TMP_Text score;
    private void Start()
    {
        score.text = "Highscore : " + PlayerPrefs.GetInt("Highscore");
    }
    public void resetScore()
    {
        PlayerPrefs.SetInt("Highscore", 0);
    }
    public void returnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
