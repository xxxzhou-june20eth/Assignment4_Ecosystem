using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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

    public enum MonkeyPosition{
        OnTruck,
        OnBranch,
        NotInPicture
    }
    public MonkeyPosition _currentPosition;
    public UnityEngine.Vector2 Position;
    // Start is called before the first frame update
    void Start()
    {
        _currentState=MonkeyState.Despawn;
        Position=new UnityEngine.Vector2(this.transform.position.x,this.transform.position.y);
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
        float waitTime=Random.Range(2,4);
        float walkTime=Random.Range(1,3);
    }

    void MoveToward(UnityEngine.Vector3 target){ //Climb up and down on current big branch
        if (target!=null){
            UnityEngine.Vector3 verticalMovement=target - transform.position;
            this.transform.position += verticalMovement * 0.2f;
        }
    }

    void Eating(){ //stop moving and eat some banana

    }

    void Despawning(){ //despawn
        UnityEngine.Vector2 leftEnd=new UnityEngine.Vector2 (-1,-1);
        UnityEngine.Vector2 rightEnd=new UnityEngine.Vector2 (1,1);
        if (Position.x<=0){
            MoveToward(leftEnd);
        }
        else{
            MoveToward(rightEnd);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        Position=new UnityEngine.Vector2(this.transform.position.x,this.transform.position.y);
        switch (_currentState){

        }
    }
}
