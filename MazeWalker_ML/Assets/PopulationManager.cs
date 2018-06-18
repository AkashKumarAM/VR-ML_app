using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour
{

    public GameObject botPrefab;
    public GameObject startingPos;
    public int populationSize = 50;
    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;
    public float trailtime = 5;
    private int generation = 1;


    GUIStyle guiStyle = new GUIStyle();

    void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "GEN" + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), String.Format("TIME:{0:0.00}", elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population" + population.Count, guiStyle);
        GUI.EndGroup();
    }


    // Use this for initialization
    void Start()
    {

        for (int i = 0; i < populationSize; i++)
        {
            
            GameObject b = Instantiate(botPrefab, startingPos.transform.position, this.transform.rotation);
            b.GetComponent<Brain>().InIt();
            population.Add(b);

        }

    }


    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        

        GameObject offSpring = Instantiate(botPrefab, startingPos.transform.position, this.transform.rotation);
        Brain b = offSpring.GetComponent<Brain>();
        if (UnityEngine.Random.Range(0, 100) == 1)
        {
            b.InIt();
            b.dna.Mutate();
        }
        else
        {
            b.InIt();
            b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);

        }

        return offSpring;
    }

    void BreedNewPopulation()
    {
        List<GameObject> sortedlist = population.OrderBy(o => o.GetComponent<Brain>().distanceTravelled).ToList();
        population.Clear();
        for (int i = (int)(sortedlist.Count / 2.0f) - 1; i < sortedlist.Count - 1; i++)
        {
            population.Add(Breed(sortedlist[i], sortedlist[i + 1]));
            population.Add(Breed(sortedlist[i + 1], sortedlist[i]));

        }

        for (int i = 0; i < sortedlist.Count; i++)
        {
            Destroy(sortedlist[i]);
        }

        generation++;

    }

    // Update is called once per frame
    void Update()
    {

        elapsed += Time.deltaTime;
        if (elapsed >= trailtime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }



    }
}
