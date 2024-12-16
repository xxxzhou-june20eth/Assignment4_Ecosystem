using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.U2D;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    public enum State
    {
        Working,
        Sleeping,
        Exercising,
        GoOut
    }
    public float speed;
    public float ExerciseTime;
    public GameObject Bed;
    public GameObject Desk;
    public GameObject Yoga;
    public GameObject Outside;
    public GameObject Destination;

    public Sprite sleep;
    public Sprite work;
    public Sprite exercise;
    public Sprite idle;
    public SpriteRenderer spriter;
    public State[] ThingsToBeDone; // Array of states for the character to execute
    public State CurrentState; // Keeps track of the current task
    public bool isBusy = false; // Flag to track if the character is busy
    public bool moving=false;

    void Start()
    {
        // Initialize the sequence of tasks
        ThingsToBeDone = new State[] { State.Working, State.Exercising, State.GoOut, State.Sleeping };
        CurrentState = ThingsToBeDone[0];
        NextState();

    }

    void Update()
    {
        if (!isBusy && !moving) // Only change states when not busy
        {
            NextState();
        }
        else{
            if (moving){
                GoTo(Destination);
            }
        }
    }

    void GoTo(GameObject destination)
    {
        UnityEngine.Vector3 movement = destination.transform.position - this.transform.position;
        if (movement.magnitude >= 1)
        {
            movement *= speed; // Move 20% of the way towards the destination
            this.transform.position += movement;
        }
        else
        {
            moving=false;
            StartTask(); // Start the task when destination is reached
        }
    }

    void StartTask()
    {
        isBusy = true; // Character is now busy
        switch (CurrentState)
        {
            case State.Working:
                StartCoroutine(Work());
                break;
            case State.Sleeping:
                StartCoroutine(Sleep());
                break;
            case State.Exercising:
                StartCoroutine(Exercise());
                break;
            case State.GoOut:
                StartCoroutine(WalkDog());
                break;
        }
    }

    void NextState()
    {
        spriter.sprite=idle;
        if (ThingsToBeDone.Length > 0)
        {
            CurrentState = ThingsToBeDone[0];
            ThingsToBeDone = ThingsToBeDone.Skip(1).ToArray();
            ChangeState(CurrentState);
            moving=true;
        }
        else
        {
            // When the task list is empty, shuffle and reset
            State[] newTasks = new State[] { State.Working, State.Exercising, State.GoOut};
            ShuffleArray(newTasks);
            ThingsToBeDone = new State[] {newTasks[0],newTasks[1],newTasks[2], State.Sleeping};
            CurrentState = ThingsToBeDone[0];
            ChangeState(CurrentState);
            moving=true;
        }
    }

    void ChangeState(State newState)
    {
        switch (newState)
        {
            case State.Working:
                Destination=Desk;
                break;
            case State.Sleeping:
                Destination=Bed;
                break;
            case State.Exercising:
                Destination=Yoga;
                break;
            case State.GoOut:
                Destination=Outside;
                break;
        }
        moving=true;
    }

    void ShuffleArray<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    IEnumerator Work(){
        spriter.sprite=work;
        float WorkTime = 0f;
        Color currentColor=GetComponent<SpriteRenderer>().color;

        while (WorkTime < 5)
        {
            float t = Time.deltaTime / 4;
            currentColor.g = Mathf.Lerp(currentColor.g, 0f, t); //change the percentage of red based on time passed;
            currentColor.b = Mathf.Lerp(currentColor.b, 0f, t);
            GetComponent<SpriteRenderer>().color = currentColor;
            WorkTime += Time.deltaTime;
            yield return null;
        }
        isBusy=false;
        GetComponent<SpriteRenderer>().color = Color.white;
        print("Work Done");
    }

    IEnumerator Exercise(){
        spriter.sprite=exercise;
        print("Start Exercising");
        float WorkTime = 0f;
        float t=0;

        while (WorkTime < 5)
        {
            if (t>ExerciseTime){
                print("!");
                t=0;
                this.transform.localScale=new UnityEngine.Vector3(3,2,1);
            }
            else{
                t+=Time.deltaTime;
                if (t>ExerciseTime/2){
                    this.transform.localScale=new UnityEngine.Vector3(3,3,1);
                }
            }
            WorkTime += Time.deltaTime;
            yield return null;
        }
        isBusy=false;
        print("Exercise Done");
    }

    IEnumerator Sleep()
    {
        spriter.sprite=sleep;
        Debug.Log("Character is sleeping.");
        this.transform.rotation = Quaternion.Euler(0, 0, 40);
        yield return new WaitForSeconds(5);
        this.transform.rotation = Quaternion.identity;
        isBusy = false;
        print("Character woke up");
    }

    IEnumerator WalkDog()
    {
        Debug.Log("Character is walking the dog.");
        yield return new WaitForSeconds(5);
        isBusy = false;
        print ("Character come back");
    }
}