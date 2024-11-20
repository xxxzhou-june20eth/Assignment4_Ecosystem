using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Serialization;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

public class CharacterBehaviour : MonoBehaviour
{
    public enum State
    {
        Working,
        Sleeping,
        Exercising,
        GoOut
    }

    public GameObject Bed;
    public GameObject Desk;
    public GameObject Yoga;
    public GameObject Outside;
    public GameObject Dog;
    public State[] ThingsToBeDone; // Array of states for the character to execute
    public State CurrentState; // Keeps track of the current task

    // Moves the character to the destination
    void GoTo(GameObject destination)
    {
        UnityEngine.Vector3 movement = destination.transform.position - this.transform.position;
        if (movement.magnitude>=1){
            movement *= 0.2f; // Move 20% of the way towards the destination
            this.transform.position += movement;
        }
        else{
            switch (CurrentState)
            {
            case State.Working:
                StartCoroutine(Work());
                Debug.Log("Character is working.");
                break;
            case State.Sleeping:
                StartCoroutine(Sleep());
                break;
            case State.Exercising:
                StartCoroutine(Exercise());
                Debug.Log("Character is exercising.");
                break;
            case State.GoOut:
                StartCoroutine(WalkDog());
                Debug.Log("Character is going out.");
                break;
            }
        }
    }

    void NextState(){
        if (ThingsToBeDone.Length>0){
            ChangeState(ThingsToBeDone[0]);
            ThingsToBeDone = ThingsToBeDone.Skip(0).ToArray();
        }
        else{
            
        }
    }
    void ChangeState(State newState)
    {
        switch (newState)
        {
            case State.Working:
                GoTo(Desk);
                break;
            case State.Sleeping:
                GoTo(Bed);
                break;
            case State.Exercising:
                GoTo(Yoga);
                break;
            case State.GoOut:
                GoTo(Outside);
                break;
        }
    }

    IEnumerator Work(){
        float WorkTime = 0f;
        Color currentColor=GetComponent<SpriteRenderer>().color;

        while (WorkTime < 5)
        {
            float t = WorkTime / 5;
            currentColor.r = Mathf.Lerp(currentColor.r, 1f, t); //change the percentage of red based on time passed;
            GetComponent<SpriteRenderer>().color = currentColor;
            WorkTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Exercise(){
        float WorkTime = 0f;
        float t=0;

        while (WorkTime < 5)
        {
            if (t>0.5f){
                t=0;
                this.transform.localScale=new UnityEngine.Vector3(1,2,1);
            }
            else{
                t+=Time.deltaTime;
                this.transform.localScale=new UnityEngine.Vector3(1,1,1);
            }
            WorkTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator WalkDog(){
        yield return new WaitForSeconds(5);
    }
    // Simulates sleeping behavior with a coroutine
    IEnumerator Sleep()
    {
        this.transform.rotation = UnityEngine.Quaternion.Euler(0, 0, 90); // Rotate to indicate sleeping
        yield return new WaitForSeconds(5);
        this.transform.rotation = UnityEngine.Quaternion.identity; //get up
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the sequence of tasks
        ThingsToBeDone = new State[] { State.Working, State.Exercising, State.GoOut, State.Sleeping };
        CurrentState=ThingsToBeDone[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
