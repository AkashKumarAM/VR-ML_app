using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    private int DNALength = 2;
    public DNA dna;
    public GameObject eyes;
    private bool seeWall = true;
    Vector3 startingPos;
    public float distanceTravelled;
    private bool alive = true;




    public void InIt()
    {
        // initialise DNA
        //0 forward
        //1 turn 

        dna = new DNA(DNALength, 360);
        startingPos = this.transform.position;
        alive = true;

    }
    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.tag == "dead")
        {
            alive = false;
            distanceTravelled = 0;
        }
    }


   

    public void Update()
    {
        if (!alive) return;

        //Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red, 10);
        seeWall = false;
        RaycastHit hit;
        if (Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.forward, out hit, 0.5f))
        {
            if (hit.collider.gameObject.tag == "wall")
            {
                seeWall = true;
            }
        }

    }


    void FixedUpdate()
    {
        if (!alive) return;

        //read DNA

        float h = 0;
        float v = dna.GetGene(0);

        if (seeWall)
        {
            h = dna.GetGene(1);
        }

        this.transform.Translate(0,0,v*0.001f);
        this.transform.Rotate(0,h,0);
        distanceTravelled = Vector3.Distance(startingPos, this.transform.position); 



    }
}
