using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NestBehaviour : MonoBehaviour
{
    public GameObject Partner;
    public GameObject Kid;
    public SpriteRenderer sr;
    public Sprite Empty;
    public Sprite Little;
    // Start is called before the first frame update
    void Start()
    {
        sr.sprite=Empty;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PartnerFlyAway(){
        Instantiate(Partner,this.transform.position,Quaternion.identity);
    }

    public void KidsGrowUp(){
        sr.sprite=Little;
        StartCoroutine(Grow());
    }

    IEnumerator Grow(){
        yield return new WaitForSeconds(5);
        sr.sprite=Empty;
        Instantiate(Kid,this.transform.position+Vector3.left*0.5f,Quaternion.identity);
        Instantiate(Kid,this.transform.position,Quaternion.identity);
    }

}
