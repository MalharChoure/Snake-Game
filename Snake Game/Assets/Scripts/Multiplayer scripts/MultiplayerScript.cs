using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MultiplayerScript : MonoBehaviour
{
    public GameObject player1_logic;
    public GameObject player2_logic;
    public GameObject snakeBody;
    private void Awake()
    {
        Snake_logic player1= player1_logic.AddComponent<Snake_logic>();
        Snake_logic player2 = player2_logic.AddComponent<Snake_logic>();
        player1.snakeBody = snakeBody;
        player2.snakeBody = snakeBody;
        player1.bodyTransform= player1_logic;
        player2.bodyTransform = player2_logic;
        player2.currentPlayer = players.player2;
        
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
}
