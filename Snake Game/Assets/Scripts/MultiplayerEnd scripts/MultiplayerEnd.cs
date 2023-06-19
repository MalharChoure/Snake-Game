using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerEnd : MonoBehaviour
{
    public TMP_Text victory;

    void Start()
    {
        victory.text = "The winner is " + PlayerPrefs.GetString("Winner");
    }

    public void toMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}


