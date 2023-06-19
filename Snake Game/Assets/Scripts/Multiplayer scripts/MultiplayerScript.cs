using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MultiplayerScript : MonoBehaviour
{
    public GameObject player1_logic;
    public GameObject player2_logic;
    public GameObject snakeBody;
    public GameObject snakeBody2;
    private void Awake()
    {
        Snake_logic player1= player1_logic.AddComponent<Snake_logic>();
        Snake_logic player2 = player2_logic.AddComponent<Snake_logic>();
        player1.snakeBody = snakeBody;
        player2.snakeBody = snakeBody2;
        player1.bodyTransform= player1_logic;
        player2.bodyTransform = player2_logic;
        player2.currentPlayer = players.player2;
    }
}
