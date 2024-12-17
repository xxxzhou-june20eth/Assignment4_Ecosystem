using System;
using UnityEngine;

public class PartnerBehaviour : MonoBehaviour
{
    public GameObject destination;
    public float speed;
    public Boolean moving;
    // Start is called before the first frame update
    void Start()
    {
        moving=true;
        destination=GameObject.Find("Tree");
    }

    // Update is called once per frame
    void Update()
    {
        if (moving){
            GoTo();
        }
    }

    void GoTo(){
        UnityEngine.Vector3 movement = destination.transform.position - this.transform.position;
        if (movement.magnitude >= 1)
        {
            movement *= speed; // Move 20% of the way towards the destination
            this.transform.position += movement;
        }
        else
        {
            moving=false;
        }
    }
}
