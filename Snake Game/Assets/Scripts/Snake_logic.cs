using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    //private vector2 for actual speed of the player
    private Vector2 _speed;

    //I plan to use a queue structure to create my snake body
    private body[] _actualBody;
    private int _tailPointer;

    // The last direction that the snake was pointed in 
    private Vector2 _lastDir = new Vector2(0, 0);

    //Timer to move snake after 1 second
    private float _timer = 0;

    //Boolean to store if a tail block needs to be added
    private bool _addTail = false;

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
            _position = _position + _speed /** Time.deltaTime*/;
            _current.transform.position = _position;
        }

        public Vector2 getPos()
        {
            return _position;
        }
    }
        // Start is called before the first frame update
    void Start()
    {
        _computeSpeed();
        _actualBody = new body[gridSizeX*gridSizeY];
        _tailPointer = -1;
        _queueHead();
        //_addQueueBody();
    }

    private void _computeSpeed() { _speed=new Vector2(1,1);}

    private void _queueHead()
    {
        if (_tailPointer == -1)
        {
            _tailPointer = 0;
            Vector2 Pos = new Vector2(1,1);//xRes/2,yRes/2);
            Vector2 Scale = new Vector2(1,1);
            Vector2 Speed= new Vector2(0,0);
            _actualBody[_tailPointer] = new body(Pos,Scale,snakeBody,Speed);
        }
    }
    private void _addQueueBody(Vector2 LastPos)
    {
        _tailPointer ++;
        Vector2 Pos = LastPos;//new Vector2(0, 1);//xRes/2,yRes/2);
        Vector2 Scale = new Vector2(1, 1);
        Vector2 Speed = new Vector2(0, 0);
        _actualBody[_tailPointer] = new body(Pos, Scale, snakeBody, Speed);
    }

    // Update is called once per frame
    void Update()
    {
        _timer = _timer + Time.deltaTime;
        if(_timer>0.25)
        {
            _timer = 0;
            _moveSnake();
            _continuousMovement();
        }
        _playerInput();
    }

    private void _playerInput()
    {
        if(_tailPointer!=-1)
        {
            int XDir=0, YDir=0;
            if(Input.GetKeyDown(KeyCode.W) && _lastDir.y>=0)
            {
                XDir = 0;
                YDir = 1;
            }
            if(Input.GetKeyDown(KeyCode.S) && _lastDir.y <= 0)
            {
                XDir = 0;
                YDir = -1;
            }
            if(Input.GetKeyDown(KeyCode.A) && _lastDir.x <= 0)
            {
                XDir = -1;
                YDir = 0;
            }
            if(Input.GetKeyDown(KeyCode.D) && _lastDir.y >= 0)
            {
                XDir = 1;
                YDir = 0;
            }
            if (XDir == 0 & YDir == 0)
            {
            }
            else
            {
                Vector2 Dir = new Vector2(XDir*_speed.x, YDir * _speed.y);
                _lastDir = Dir;
            }

            if(Input.GetKeyDown(KeyCode.F))
            {
                _addTail = true;
            }
            
        }
    }

    private void _continuousMovement()
    {
        if (_tailPointer != -1)
            _actualBody[0].changeDirection(_lastDir);
    }

    private void _moveSnake()
    {
        if (_tailPointer != -1)
        {
            if(_addTail)
            {
                _addQueueBody(_actualBody[_tailPointer].getPos());
                _addTail = false;
            }
            Debug.Log(_tailPointer);
            for (int I = _tailPointer; I > 0; I--)
            {
                Vector2 Temp = _actualBody[I - 1].getPos();
                _actualBody[I].moveBody(Temp);
                Debug.Log(I);
            }
            _actualBody[0].moveHead();
        }
    }
}
