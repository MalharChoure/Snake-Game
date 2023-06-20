using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void toSinglePlayer()
    {
        SceneManager.LoadScene("SinglePlayer");
    }

    public void toCoOP()
    {
        SceneManager.LoadScene("Multiplayer");
    }

    public void toInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void toSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void toExit()
    {
        Application.Quit();
    }
}
