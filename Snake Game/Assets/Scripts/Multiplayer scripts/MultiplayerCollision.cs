using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerCollision : MonoBehaviour
{
    private Snake_logic _player1;
    private Snake_logic _player2;
    private bool _end = false;

    // Start is called before the first frame update
    void Start()
    {
        _player1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<Snake_logic>(); 
        _player2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<Snake_logic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_end)
        {
            //Debug.Log("This is getting triggered");
            bool Snake1Death = false;
            bool Snake2Death = false;
            if (_player1 != null && _player1 != null)
            {
                Vector2[] Pos1 = _player1.returnSnakeLocation();
                Vector2[] Pos2 = _player2.returnSnakeLocation();

                for (int I = 0; I < Pos1.Length; I++)
                {
                    if (Pos2[0] == Pos1[I])
                    {
                        //Debug.Log("This is getting triggered");
                        Snake1Death = true;
                    }
                }
                for (int I = 0; I < Pos2.Length; I++)
                {
                    if (Pos1[0] == Pos2[I])
                    {
                        //Debug.Log("This is getting triggered");
                        Snake2Death = true;
                    }
                }
            }
            if (Snake1Death)
            {
                _player1.currentState = playstate.end;
                _player2.currentState = playstate.end;
                _end = true;
            }
            else if (Snake2Death)
            {
                _player1.currentState = playstate.end;
                _player2.currentState = playstate.end;
                _end = true;
            }
        }
    }
}
