using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ItemPickUpScript : MonoBehaviour
{
    public int scoreModifier = 2;
    public int scoreAdded = 1;

    //handle to call score script
    private ScoreScript _score;

    public enum items
    {
        massGainer,
        massBurner,
        shield,
        speed,
        score
    }
    //handle to call snake logic script
    private Snake_logic _script;
    public float timer = 5.0f;

    // item choices based on which item this represents
    public items itemChoice=items.massGainer;
    private void Start()
    {
        _script = GameObject.FindGameObjectWithTag("SnakeScript").GetComponent<Snake_logic>();
        _score = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScript>();
        StartCoroutine(_deathTimer());
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (_script != null)
        {
            switch ((int)itemChoice)
            {
                case 0:
                    _script.massGainerScript();
                    if(_script.scoreBoostUp)
                    {
                        _score.increaseScore(scoreAdded*scoreModifier);
                    }
                    else
                    {
                        _score.increaseScore(scoreAdded);
                    }
                    break;

                case 1:
                    _script.massBurnerScript();
                    if (_script.scoreBoostUp)
                    {
                        _score.increaseScore(scoreAdded*scoreModifier);
                    }
                    else
                    {
                        _score.increaseScore(scoreAdded);
                    }
                    break;

                case 2:
                    _script.shieldScript();
                    break;

                case 3:
                    _script.speedScript();
                    break;

                case 4:
                    _script.scoreBoostScript();
                    break;
            }
        }
            //_script.GetComponent<Snake_logic>().massGainerScript();
        Destroy(gameObject);
    }

    private IEnumerator _deathTimer()
    {
        //Debug.Log("test");
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
        yield return null;
    }
}
