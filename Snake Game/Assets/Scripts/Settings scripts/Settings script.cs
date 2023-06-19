using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settingsscript : MonoBehaviour
{
    // Start is called before the first frame update

    public void resetScore()
    {
        PlayerPrefs.SetInt("Highscore", 0);
    }

    public void returnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
