
using System.Collections;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
    public enum SingleState
    {
        Flying,
        Knocking,
        GoOut
    }
    public enum DoubleState
    {
        GoHome,
        Loving,
        Egging,
        GoOut
    }
    public float speed;
    public float KnockTime;
    public float FlyTime;
    public float FlyForce;
    public Vector3 maximumPosition;
    public GameObject Tree;
    public GameObject Nest;
    public GameObject Air;
    public GameObject Destination;
    public GameObject Partner;
    public Sprite Fly;
    public Sprite Knock;
    public Sprite Idle;
    public Sprite DoubleFly;
    public Sprite Love;
    public Sprite Egg;
    public SpriteRenderer spriter;
    public SingleState[] ThingsToBeDone; // Array of states for the character to execute
    public bool Single;
    public SingleState SingleCurrentState; // Keeps track of the current task
    public DoubleState DoubleCurrentState;
    public DoubleState[] LoveProcess={DoubleState.GoHome,DoubleState.Loving,DoubleState.Egging,DoubleState.GoOut};
    public bool isBusy = false; // Flag to track if the character is busy
    public bool moving=false;
    public bool Next=true;
    void Start()
    {
        // Initialize the sequence of tasks
        ThingsToBeDone = new SingleState[] {SingleState.Flying, SingleState.Knocking, SingleState.GoOut};
        SingleCurrentState = ThingsToBeDone[0];
        DoubleCurrentState = LoveProcess[0];
        Single=true;
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
        if (Single){
            switch (SingleCurrentState)
            {
                case SingleState.Flying:
                    StartCoroutine(Flying());
                    break;
                case SingleState.Knocking:
                    StartCoroutine(KnockWindow());
                    break;
                case SingleState.GoOut:
                    StartCoroutine(Wait(5));
                    break;
            }
        }
        else{
            switch (DoubleCurrentState)
            {
                case DoubleState.GoHome:
                    spriter.sprite=Idle;
                    StartCoroutine(Wait(5));
                    break;
                case DoubleState.Loving:
                    spriter.sprite=Love;
                    StartCoroutine(Wait(3));
                    break;
                case DoubleState.Egging:
                    Nest.GetComponent<NestBehaviour>().PartnerFlyAway();
                    spriter.sprite=Egg;
                    StartCoroutine(Wait(6));
                    break;
                case DoubleState.GoOut:
                    Nest.GetComponent<NestBehaviour>().KidsGrowUp();
                    StartCoroutine(Wait(10));
                    break;
            }
        }
    }

    void NextState()
    {
        if (Single){
            spriter.sprite=Idle;
        }
        else{
            spriter.sprite=DoubleFly;
        }

        if (Single){
            if (ThingsToBeDone.Length > 0)
            {
                SingleCurrentState = ThingsToBeDone[0];
                ThingsToBeDone = ThingsToBeDone.Skip(1).ToArray();
                ChangeSingleState(SingleCurrentState);
                moving=true;
            }
            else
            {
                int I=UnityEngine.Random.Range(0,10);
                if (I<7){
                    Single=true;
                    // When the task list is empty, shuffle and reset
                    SingleState[] newTasks = new SingleState[] {SingleState.GoOut,SingleState.Flying, SingleState.Knocking,SingleState.GoOut};
                    ShuffleArray(newTasks);
                    ThingsToBeDone = newTasks;
                    SingleCurrentState = ThingsToBeDone[0];
                    ChangeSingleState(SingleCurrentState);
                }
                else{
                    Single=false;
                    DoubleCurrentState=DoubleState.GoHome;
                    ChangeDoubleState(DoubleCurrentState);
                }
                moving=true;
            }
        }
        else{
            if (DoubleCurrentState<=DoubleState.GoOut)
            {
                DoubleCurrentState+=1;
                ChangeDoubleState(DoubleCurrentState);
                moving=true;
            }
            else
            {
                Single=true;
                DoubleCurrentState=DoubleState.GoHome;
                // When the task list is empty, shuffle and reset
                SingleState[] newTasks = new SingleState[] {SingleState.GoOut,SingleState.Flying, SingleState.Knocking,SingleState.GoOut};
                ShuffleArray(newTasks);
                ThingsToBeDone = newTasks;
                SingleCurrentState = ThingsToBeDone[0];
                ChangeSingleState(SingleCurrentState);
                moving=true;
            }
        }
    }

    void ChangeSingleState(SingleState newState)
    {
        switch (newState)
        {
            case SingleState.Flying:
                Destination=Air;
                break;
            case SingleState.Knocking:
                Destination=Nest;
                break;
            case SingleState.GoOut:
                Destination=Tree;
                break;
        }
        moving=true;
    }

    void ChangeDoubleState(DoubleState newState)
    {
        switch (newState)
        {
            case DoubleState.GoHome:
                Destination=Tree;
                break;
            case DoubleState.Loving:
                Destination=Nest;
                break;
            case DoubleState.Egging:
                Destination=Nest;
                break;
            case DoubleState.GoOut:
                Destination=Tree;
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

    IEnumerator KnockWindow(){
        print("BirdKnock");
        spriter.sprite=Knock;
        float WorkTime = 0f;
        Color currentColor=GetComponent<SpriteRenderer>().color;
        float t=0;

        while (WorkTime < 7)
        {
            if (t>KnockTime){
                t=0;
                this.transform.rotation=Quaternion.Euler(45,0,0);
            }
            else{
                t+=Time.deltaTime;
                if (t>KnockTime/2){
                    this.transform.rotation=quaternion.identity;
                }
            }
            WorkTime += Time.deltaTime;
            yield return null;
        }
        isBusy=false;
        this.transform.rotation=quaternion.identity;
        print("Bird Knock Done");
    }

    IEnumerator Flying(){
        spriter.sprite=Fly;
        print("BirdFly");
        float WorkTime = 0f;
        float t=0;

        while (WorkTime < 3)
        {
            if (t>FlyTime){
                t=0;
            }
            else{
                if (t>FlyTime/2){
                    this.transform.position=Vector3.Lerp(this.transform.position,maximumPosition,0.2f);
                }
                else{
                    this.transform.position=Vector3.Lerp(this.transform.position,Air.transform.position,0.2f);
                }
                t+=Time.deltaTime;
            }
            WorkTime += Time.deltaTime;
            yield return null;
        }
        isBusy=false;
        print("BirdFly Done");
    }

    IEnumerator Wait(int a)
    {
        Debug.Log("Bird wait for "+a.ToString()+"Seconds.");
        yield return new WaitForSeconds(a);
        if (DoubleCurrentState==DoubleState.GoHome){
            Single=true;
        }
        isBusy = false;
        print("End wait");
    }


}