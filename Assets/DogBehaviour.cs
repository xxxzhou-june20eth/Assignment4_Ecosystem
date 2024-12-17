
using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class DogBehaviour : MonoBehaviour
{
    public enum State
    {
        Eating,
        Sleeping,
        Playing,
        Jumping,
        GoOut
    }
    public float speed;
    public float eatTime;
    public float jumpTime;
    public float jumpForce;
    public Vector3 maximumPosition;
    public GameObject DogBed;
    public GameObject HumanBed;
    public GameObject Table;
    public GameObject Outside;
    public GameObject Destination;
    public CharacterBehaviour Human;
    public Sprite sleep;
    public Sprite eat;
    public Sprite jump;
    public Sprite idle;
    public SpriteRenderer spriter;
    public State[] ThingsToBeDone; // Array of states for the character to execute
    public State CurrentState; // Keeps track of the current task
    public bool isBusy = false; // Flag to track if the character is busy
    public bool moving=false;

    void Start()
    {
        // Initialize the sequence of tasks
        ThingsToBeDone = new State[] {State.Eating,  State.Jumping, State.GoOut, State.Sleeping};
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
            case State.Sleeping:
                StartCoroutine(Sleep());
                break;
            case State.Eating:
                StartCoroutine(Eat());
                break;
            case State.GoOut:
                StartCoroutine(WalkDog());
                break;
            case State.Jumping:
                StartCoroutine(Jump());
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
            State[] newTasks = new State[] {State.Eating, State.Jumping,State.Sleeping};
            ShuffleArray(newTasks);
            int i=0;
            while (Human.ThingsToBeDone[i]!=CharacterBehaviour.State.GoOut){
                i+=1;
            }
            ThingsToBeDone = new State[] {newTasks[0],newTasks[1],State.GoOut,newTasks[2]};
            State temp=ThingsToBeDone[i];
            ThingsToBeDone[i]=State.GoOut;
            ThingsToBeDone[2]=temp;
            CurrentState = ThingsToBeDone[0];
            ChangeState(CurrentState);
            moving=true;
        }
    }

    void ChangeState(State newState)
    {
        switch (newState)
        {
            case State.Eating:
                Destination=Table;
                break;
            case State.Sleeping:
                Destination=DogBed;
                break;
            case State.Jumping:
                Destination=HumanBed;
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

    IEnumerator Eat(){
        print("Dog eat");
        spriter.sprite=eat;
        float WorkTime = 0f;
        Color currentColor=GetComponent<SpriteRenderer>().color;
        float t=0;

        while (WorkTime < 5)
        {
            if (t>eatTime){
                t=0;
                this.transform.rotation=Quaternion.Euler(0,0,15);
            }
            else{
                t+=Time.deltaTime;
                if (t>eatTime/2){
                    this.transform.rotation=quaternion.identity;
                }
            }
            WorkTime += Time.deltaTime;
            yield return null;
        }
        isBusy=false;
        this.transform.rotation=quaternion.identity;
        print("Eat Done");
    }

    IEnumerator Jump(){
        spriter.sprite=jump;
        print("Dog Jump");
        float WorkTime = 0f;
        float t=0;

        while (WorkTime < 5)
        {
            if (t>jumpTime){
                t=0;
            }
            else{
                if (t>jumpTime/2){
                    this.transform.position=Vector3.Lerp(this.transform.position,maximumPosition,0.2f);
                }
                else{
                    this.transform.position=Vector3.Lerp(this.transform.position,HumanBed.transform.position,0.2f);
                }
                t+=Time.deltaTime;
            }
            WorkTime += Time.deltaTime;
            yield return null;
        }
        isBusy=false;
        print("Jump Done");
    }

    IEnumerator Sleep()
    {
        spriter.sprite=sleep;
        Debug.Log("Dog sleep.");
        yield return new WaitForSeconds(5);
        isBusy = false;
        print("Dog woke up");
    }

    IEnumerator WalkDog()
    {
        Debug.Log("Dog walk.");
        yield return new WaitForSeconds(5);
        isBusy = false;
        print ("Dog come back");
    }
}