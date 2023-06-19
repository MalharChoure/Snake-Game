using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endScriptSinglePlayer : MonoBehaviour
{
    public TMP_Text score;

    void Start()
    {
        score.text = "Score : " + PlayerPrefs.GetInt("Score");
    }

    public void toMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
