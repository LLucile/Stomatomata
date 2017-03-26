using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feeder : MonoBehaviour {

    private bool eating = false;
    private List<GameObject> meal;
    private GameObject currentBrochette = null;
    private int currentAlimentIndex = 0;
    private GameObject currentAliment = null;

    private int currentLevel = 0;

    public float timeToWait = 1f;
    private float timer = 0;

    ParseLevels parser;

	// Use this for initialization
	void Start () {
        meal = new List<GameObject>();
        parser = GameObject.FindObjectOfType<ParseLevels>();
	}
	
	// Update is called once per frame
	void Update () {
        if (eating)
        {
            if (timer > timeToWait)
            {
                //Debug.Log( meal.Count + " borchettes left");
                if (meal.Count == 0)
                {
                    Debug.Log("no brochette left");
                    eating = false;
                    currentBrochette = null;
                }
                else
                {
                    if (0 == currentAlimentIndex)
                    {
                        currentBrochette = meal[0];
                        meal.Remove(currentBrochette);
                        currentAlimentIndex = currentBrochette.transform.childCount - 1;
                    }
                    else
                    {
                        currentAliment = currentBrochette.transform.GetChild(currentAlimentIndex).gameObject;
                        Debug.Log("picked Aliment " + currentAliment.name + " in brochette " + currentBrochette.name);
                        currentAlimentIndex--;
                    }
                }
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
	}

    public void EatNow()
    {
        //Copy list of brochettes into meal;
        int i;
        for(i = 0; i < parser.levelsGameObjects[currentLevel].transform.childCount; i++){
            meal.Add(parser.levelsGameObjects[currentLevel].transform.GetChild(i).gameObject);
        }
        Debug.Log("prepared " + i + "to eat");
        eating = true;
        //set first brochette
        currentBrochette = meal[0];
        currentAlimentIndex = currentBrochette.transform.childCount -1;
    }




}
