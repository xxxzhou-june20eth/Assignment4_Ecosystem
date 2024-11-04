using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MonkeyBehavior : MonoBehaviour
{
    public enum MonkeyState{
        Climbing,
        Entering,
        Leaving,
        Eating,
        Hanging,
        Walking,
        Despawn
    }
    public float speed;
    public MonkeyState _previousState;
    public MonkeyState _currentState;
    public MonkeyState _nextState;
    public Vector2 Position;
    // Start is called before the first frame update
    void Start()
    {
        _currentState=MonkeyState.Despawn;
        Position=new Vector2(this.transform.position.x,this.transform.position.y);
    }

    IEnumerator Wait(int time){
        yield return new WaitForSeconds(time);
    }

    void Spawning(){ //wait for 3-10 seconds then spawn
        int waitTime=Random.Range(3,10);
        Wait(waitTime);
        _nextState=MonkeyState.Entering;
    }

    void Walking(){ //walking left and right on current small branch

    }

    void Climbing(Vector2 target){ //Climb up and down on current big branch
        if (target!=null){

        }
    }

    void Eating(){ //stop moving and eat some banana

    }

    void Despawning(){ //despawn
        Vector2 leftEnd=new Vector2 (-1,-1);
        Vector2 rightEnd=new Vector2 (1,1);
        if (Position.x<=0){
            Climbing(leftEnd);
        }
        else{
            Climbing(rightEnd);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        Position=new Vector2(this.transform.position.x,this.transform.position.y);
        
    }
}
