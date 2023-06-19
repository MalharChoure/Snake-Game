using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
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
    private float _timerCopy;
    private float _timerLimit = 0.25f;

    //Boolean to store if a tail block needs to be added
    private bool _addTail = false;

    //parent gameobject
    public GameObject bodyTransform;

    //the game arena boundry 
    public int upperBound = 4;
    public int lowerBound = -4;
    public int leftBound = -8;
    public int rightBound = 8;
    // These variables will determine the boundry of the arena allowing me to warp

    //Boolean to record collision between snake head and body
    private bool _collision = false;

    //Boolean to check if shield is up
    private bool _shieldUp = false;
    public float shieldTimer = 5.0f;

    //Enumerator used to record state of play
    public playstate currentState=playstate.start;

    // speed increase timer
    public float speedTimer = 10.0f;

    //Score boost enable bool and modifier
    public bool scoreBoostUp = false;
    public float scoreTimer= 10.0f;
    //public int scoreBoostMultiplier = 2;

    //boolean to have input change only once per input action
    private bool _directionSelect=false;

    //This is to add the two player effect
    public players currentPlayer = players.player1;

    public class body
    {
        private Vector2 _position;
        private Vector2 _scale;
        private Vector2 _speed;
        private GameObject _snakeBody;
        private GameObject _current;


        public body(Vector2 Start, Vector2 Scale, GameObject Obj,Vector2 Speed,GameObject ObjTransform)
        {
            _position = Start;
            this._scale = Scale;
            _snakeBody = Obj;
            _current = Instantiate(_snakeBody, _position, Quaternion.identity,ObjTransform.transform);
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
            _position = _position + _speed;
            _current.transform.position = _position;
        }

        public Vector2 getPos()
        {
            return _position;
        }

        public void destroySelfBody()
        {
            Destroy(_current);
        }

        public void changeShieldColourYellow()
        {
            Debug.Log("Should be here");
            _current.GetComponent<SpriteRenderer>().material.color = Color.gray;
        }

        public void changeShieldColourGreen()
        {
            Debug.Log("Come here");
            _current.GetComponent<SpriteRenderer>().material.color = Color.green;
        }
    }

    void Start()
    {
        _timerCopy = _timerLimit;
        _computeSpeed();
        _actualBody = new body[gridSizeX*gridSizeY];
        _tailPointer = -1;
        _queueHead();
    }

    private void _computeSpeed() { _speed=new Vector2(1,1);}//I have kept this incase i need to compute speed in a different way depending on arena size but since speed is being determined by a timer this is redundant

    private void _queueHead()
    {
        if (_tailPointer == -1 && currentPlayer==players.player1)
        {
            _tailPointer = 0;
            Vector2 Pos = new Vector2(-3,0);
            Vector2 Scale = new Vector2(1,1);
            Vector2 Speed= new Vector2(0,0);
            _actualBody[_tailPointer] = new body(Pos,Scale,snakeBody,Speed,bodyTransform);
        }
        else if(_tailPointer == -1 && currentPlayer == players.player2)
        {
            _tailPointer = 0;
            Vector2 Pos = new Vector2(3, 0);
            Vector2 Scale = new Vector2(1, 1);
            Vector2 Speed = new Vector2(0, 0);
            _actualBody[_tailPointer] = new body(Pos, Scale, snakeBody, Speed, bodyTransform);
        }
    }
    private void _addQueueBody(Vector2 LastPos)
    {
        _tailPointer ++;
        Vector2 Pos = LastPos;
        Vector2 Scale = new Vector2(1, 1);
        Vector2 Speed = new Vector2(0, 0);
        _actualBody[_tailPointer] = new body(Pos, Scale, snakeBody, Speed, bodyTransform);
    }

    // Update is called once per frame
    void Update()
    {
        _timer = _timer + Time.deltaTime;
        if(_timer>_timerLimit)
        {
            _timer = 0;
            if (!_collision)
            {
                _moveSnake();
                _continuousMovement();
                _selfCollisionCheck();
                _boundaries();
            }
            else if(_tailPointer>-1)
            {
                _destroySnakeBodyPart();
            }
            else
            {
                _tailPointer = -1;
                _collision = false;
                currentState = playstate.end;
            }
            if(currentState==playstate.end)
            {
                _collision= true;
            }
        }
        _playerInput();
    }

    private void _playerInput()
    {

        if (_tailPointer!=-1 && !_directionSelect)
        {

            int XDir=0, YDir=0;
            if (currentPlayer == players.player1)
            {
                if (Input.GetKeyDown(KeyCode.W) && _lastDir.y >= 0)
                {
                    XDir = 0;
                    YDir = 1;
                    _directionSelect = true;
                }
                if (Input.GetKeyDown(KeyCode.S) && _lastDir.y <= 0)
                {
                    XDir = 0;
                    YDir = -1;
                    _directionSelect = true;
                }
                if (Input.GetKeyDown(KeyCode.A) && _lastDir.x <= 0)
                {
                    XDir = -1;
                    YDir = 0;
                    _directionSelect = true;
                }
                if (Input.GetKeyDown(KeyCode.D) && _lastDir.x >= 0)
                {
                    XDir = 1;
                    YDir = 0;
                    _directionSelect = true;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && _lastDir.y >= 0)
                {
                    XDir = 0;
                    YDir = 1;
                    _directionSelect = true;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) && _lastDir.y <= 0)
                {
                    XDir = 0;
                    YDir = -1;
                    _directionSelect = true;
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow) && _lastDir.x <= 0)
                {
                    XDir = -1;
                    YDir = 0;
                    _directionSelect = true;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow) && _lastDir.x >= 0)
                {
                    XDir = 1;
                    YDir = 0;
                    _directionSelect = true;
                }
            }
            if (XDir == 0 & YDir == 0)
            {
            }
            else
            {
                currentState=playstate.inplay;
                Vector2 Dir = new Vector2(XDir*_speed.x, YDir * _speed.y);
                _lastDir = Dir;
            }

            /*if(Input.GetKeyDown(KeyCode.F))
            {
                _addTail = true;
            }*/
            
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
                if(_shieldUp)
                _actualBody[_tailPointer].changeShieldColourYellow();
            }
            for (int I = _tailPointer; I > 0; I--)
            {
                Vector2 Temp = _actualBody[I - 1].getPos();
                _actualBody[I].moveBody(Temp);
            }
            _actualBody[0].moveHead();
            _directionSelect = false;
        }
    }

    private void _selfCollisionCheck()
    {
        if (_tailPointer >0 && !_shieldUp)
        {
            Vector2 Head = _actualBody[0].getPos();
            for (int I = 1; I < _tailPointer; I++)
            {
                if (Head == _actualBody[I].getPos())
                {
                    _collision = true;
                    currentState = playstate.end;
                    break;
                }
            }

        }
    }
    

    private void _destroySnakeBodyPart()
    {
        _actualBody[_tailPointer].destroySelfBody();
        _actualBody[_tailPointer] = null;
        _tailPointer--;
    }

    private void _boundaries()
    {
        if (_tailPointer >= 0)
        {
            Vector2 Pos = _actualBody[0].getPos();
            if (Pos.x > rightBound)
            {
                Pos.x = leftBound;
            }
            if (Pos.x < leftBound)
            {
                Pos.x = rightBound;
            }
            if (Pos.y > upperBound)
            {
                Pos.y = lowerBound;
            }
            if (Pos.y < lowerBound)
            {
                Pos.y = upperBound;
            }
            _actualBody[0].moveBody(Pos);
        }
    }

    // All function beyond this are called by various other scripts
    public void massBurnerScript()
    {
        if (_tailPointer > 0)
        {
            _destroySnakeBodyPart();
        }
    }

    public void massGainerScript()
    {
        _addTail = true;
    }

    public Vector2[] returnSnakeLocation()
    {
        Vector2[] Positions = new Vector2[_tailPointer+1];
        for(int I=0;I<=_tailPointer;I++)
        {
            Positions[I]= _actualBody[I].getPos();
        }
        return Positions;
    }

    public void shieldScript()
    {
        _shieldUp = true;
        StartCoroutine(shieldUp());
    }

    public void speedScript()
    {
        _timerLimit = 0.05f;
        StartCoroutine(speedBoostUp());
    }

    public void scoreBoostScript()
    {
        scoreBoostUp = true;
        StartCoroutine(scoreBoost());
    }

    private IEnumerator shieldUp()
    {
        shieldColorChange();
        yield return new WaitForSeconds(shieldTimer);
        shieldColorRevert();
        _shieldUp = false;
        yield return null;
    }

    private IEnumerator speedBoostUp()
    {
        yield return new WaitForSeconds(speedTimer);
        _timerLimit = _timerCopy;
        yield return null;
    }

    private IEnumerator scoreBoost()
    {
        yield return new WaitForSeconds(scoreTimer);
        scoreBoostUp = false;
        yield return null;
    }

    private void shieldColorChange()
    {
        for(int I=0;I<=_tailPointer;I++)
        {
            _actualBody[I].changeShieldColourYellow();
        }
    }

    private void shieldColorRevert()
    {
        for (int I = 0; I <=_tailPointer; I++)
        {
            _actualBody[I].changeShieldColourGreen();
        }
    }
}
