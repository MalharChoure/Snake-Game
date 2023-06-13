using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_logic : MonoBehaviour
{
    // The prefab that is going to form the body of the snake
    public GameObject snakeBody;

    // The size of the gamemap
    public int gridSizeX = 100;
    public int gridSizeY = 100;

    // The speed of the player
    public int speedModifier = 1;

    // The size of each square
    public int xRes = 1920;
    public int yRes = 1080;

    //private int for actual speed of the player

    public class body
    {
        private Vector2 _position;
        private Vector2 _scale;
        private Vector2 _speed;
        private GameObject _snakeBody;
        private GameObject _current;


        public body(Vector2 Start, Vector2 Scale, GameObject Obj,Vector2 Speed)
        {
            _position = Start;
            this._scale = Scale;
            _snakeBody = Obj;
            _current = Instantiate(_snakeBody, _position, Quaternion.identity);
            _changeScale();
            _speed= Speed;
        }

        public void changeDirection(Vector2 _newDirection)
        {
            _speed = _newDirection;
        }

        private void _changeScale()
        {
            _current.transform.localScale = _scale;
        }

        public void moveBody(Vector2 _currentDirection)
        {
            _position = _currentDirection;
            _current.transform.position = _position;
        }

        public void moveHead()
        {
            _position = _position + _speed * Time.deltaTime;
            _current.transform.position = _position;
        }
    }
        // Start is called before the first frame update
    void Start()
    {


        Vector2 Pos = new Vector2(1,2);
        Vector2 Scale = new Vector2(1,2);
        body a = new body(Pos,Scale,snakeBody);
    }

    private void _computeSpeed() { }

    // Update is called once per frame
    void Update()
    {
        
    }
}
