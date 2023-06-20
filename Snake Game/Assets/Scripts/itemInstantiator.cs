using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class itemInstantiator : MonoBehaviour
{

    private enum itemType
    {
        food,
        powerUp
    }

    //bools to check if one is spawned
    private bool _foodSpawned=false;
    private bool _powerUpSpawned=false;

    //the prefabs for the various items;
    public GameObject[] food;
    public GameObject[] powerup;

    //timers for spawning the items;
    private float _foodTimer=0;
    private float _powerTimer=0;
    private bool _foodTimerOn=true;
    private bool _powerTimerOn=true;

    //spawn times should be changable
    public float foodSpawnTimer = 2.0f;
    public float powerSpawnTimer = 2.0f;

    //handle for getting the Snake logic script to make sure the powerups do not spawn on it.
    private Snake_logic _snakeLogicscript;

    //Coroutine timer
    public float timer=5.0f;

    private void Start()
    {
        _snakeLogicscript= GameObject.FindGameObjectWithTag("SnakeScript").GetComponent<Snake_logic>();
        if(_snakeLogicscript==null)
        {
            Debug.Log("Could not get snake logic script component");
        }
    }
    private void Update()
    {
        if (_snakeLogicscript.currentState == playstate.inplay)
        {
            if (!_foodSpawned)
            {
                _foodSpawned = true;

            }
            if (!_powerUpSpawned)
            {
                _powerUpSpawned = true;
            }
            if (_foodTimerOn)
            {
                _foodTimer += Time.deltaTime;
            }
            if (_powerTimerOn)
            {
                _powerTimer += Time.deltaTime;
            }
            if (_foodTimer > foodSpawnTimer)
            {
                int SpawnRatio = Random.Range(0, 20);
                int rng = SpawnRatio > 3 ? 1 : 0;//Random.Range(0, 2);
                _foodTimerOn = false;
                StartCoroutine(_spawnItem(food[rng], itemType.food));
                Debug.Log("Coroutine for food started");
                _foodTimer = 0;
            }
            if (_powerTimer > powerSpawnTimer)
            {
                int rng = Random.Range(0, 3);
                _powerTimerOn = false;
                StartCoroutine(_spawnItem(powerup[rng], itemType.powerUp));
                Debug.Log("Coroutine for powerup started");
                _powerTimer = 0;
            }
        }
    }

    private IEnumerator _spawnItem(GameObject Obj,itemType Item)
    {
        bool Flag = true;
        Vector2 Pos=new Vector2(0,0);
        Debug.Log("Inside Coroutine");
        while(Flag)
        {
            Pos = new Vector2(Random.Range(_snakeLogicscript.leftBound,_snakeLogicscript.rightBound),Random.Range(_snakeLogicscript.lowerBound,_snakeLogicscript.upperBound));
            Debug.Log(Pos);
            if(_returnValidPosition(Pos))
            {
                Flag = false;
                break;
            }
        }
        Instantiate(Obj,Pos,Quaternion.identity);       
        yield return new WaitForSeconds(timer);
        Debug.Log("After Delay");
        if (Item==itemType.food)
        {
            _foodTimerOn = true;
        }
        if(Item==itemType.powerUp)
        {
            _powerTimerOn = true;
        }
 
        yield return null;
    }

    private bool _returnValidPosition(Vector2 Pos)
    {
        bool BrokenLoop = false;
        Vector2[] SnakePos=_snakeLogicscript.returnSnakeLocation();
        for(int I=0;I<SnakePos.Length;I++)
        {
            if (SnakePos[I]==Pos) 
            { 
                BrokenLoop= true; break;    
            }
        }
        return !BrokenLoop;
    }    
}
