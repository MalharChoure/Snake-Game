using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    private TMP_Text _scoreHandler;
    private int _scoreText = 0;
    // Start is called before the first frame update
    void Start()
    {
        _scoreHandler = gameObject.GetComponent<TMP_Text>();
        if (_scoreHandler != null)
        {
            _scoreHandler.text = "Score: " + _scoreText;
        }
        else
        {
            Debug.Log("The _scoreHandler component could not be found");
        }
    }

    // Update is called once per frame
    public void increaseScore(int Increase)
    {
        _scoreText += Increase;
        _scoreHandler.text = "Score: " + _scoreText;
    }
}
