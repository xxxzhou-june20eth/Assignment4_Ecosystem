using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class KidBehaviour : MonoBehaviour
{
    public GameObject destination;
    public float speed;
    public Boolean moving;
    public Sprite fly;
    public SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GrowUp());
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

    IEnumerator GrowUp(){
        yield return new WaitForSeconds(5);
        sr.sprite=fly;
        moving=true;
    }
}
