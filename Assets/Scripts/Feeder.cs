﻿using System.Collections;
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
    private int mealCount = 0;

    public bool processed = false;

    public GameObject mouthNode;
    public GameObject stomachNode;

    private GameObject currentNode;
    private int transitionsCount = 0;
    private GameObject transition = null;

    public List<bool> results;

    ParseLevels parser;

	// Use this for initialization
	void Start () {
        meal = new List<GameObject>();
        parser = GameObject.FindObjectOfType<ParseLevels>();
	}
	
	// Update is called once per frame
	void Update () {
        //If eat function as been called and eating process is not done yet
        if (eating)
        {
            if (timer > timeToWait)
            {
                //Debug.Log( mealCount + " brochettes left");
                //if there is nothing left to eat
                if (mealCount == 0)
                {
                    Debug.Log("no brochette left");
                    eating = false;
                    processed = true;
                    currentBrochette = null;
                }
                else
                {
                    //if there is no aliment left on the brochette
                    if (0 == currentAlimentIndex)
                    {
                        //La brochette a bien été digérée :
                        results.Add(true);
                        mealCount--;
                        if (mealCount > 0)
                        {
                            currentBrochette = meal[0];
                            meal.Remove(currentBrochette);
                            currentAlimentIndex = currentBrochette.transform.childCount - 1;
                            currentNode = mouthNode;
                        }
                    }
                    else
                    {
                        //pick first aliment
                        currentAliment = currentBrochette.transform.GetChild(currentAlimentIndex).gameObject;
                        Debug.Log("picked Aliment " + currentAliment.name + " in brochette " + currentBrochette.name);
                        //Let's try to add a shader effect
                        currentAliment.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(1f, 1f, 1f, 0.5f)); 
                        currentAlimentIndex--;
                        //TODO grey current node
                        // will require to use several sprites
                        //Do the testing
                            //Check if any transitions of current Node allows our aliment to travel
                        bool isValid = false;
                        for (int i = 0; i < currentNode.GetComponent<Node>().transitions.Count; i++)
                        {
                            if ((int)currentNode.GetComponent<Node>().transitions[i].GetComponent<Transition>().transitionType == (int)currentAliment.GetComponent<NodeType>().type)
                            {
                                isValid = true;
                            }
                            else if (currentNode.GetComponent<Node>().transitions[i].GetComponent<Transition>().transitionType == Transition.tType.All)
                            {
                                isValid = true;
                            }
                            else if( (currentNode.GetComponent<Node>().transitions[i].GetComponent<Transition>().transitionType == Transition.tType.NotRed  && currentAliment.GetComponent<NodeType>().type != NodeType.tType.Red) 
                                || (currentNode.GetComponent<Node>().transitions[i].GetComponent<Transition>().transitionType == Transition.tType.NotBlue  && currentAliment.GetComponent<NodeType>().type != NodeType.tType.Blue)
                                || (currentNode.GetComponent<Node>().transitions[i].GetComponent<Transition>().transitionType == Transition.tType.NotGreen  && currentAliment.GetComponent<NodeType>().type != NodeType.tType.Green)
                                )
                            {
                                isValid = true;
                            }
                            if(isValid){
                                transition = currentNode.GetComponent<Node>().transitions[i];
                            }
                            isValid = false;
                        }
                        if(transition == null){
                            //fail : set color aliment slight red
                            Debug.Log("Cet aliment n'est pas digéré");
                            //Resultat de la brochette : false. 
                            results.Add(false);
                            //Passer à la brochette suivante 
                            mealCount--;
                            if (mealCount > 0)
                            {
                                currentBrochette = meal[0];
                                meal.Remove(currentBrochette);
                                currentAlimentIndex = currentBrochette.transform.childCount - 1;
                                currentNode = mouthNode;
                            }
                        }
                        else{
                            //travel aliment : set color aliment slight green
                            transitionsCount = 0;
                            Debug.Log("aliment digéré avec succès");
                            currentNode = transition.GetComponent<Transition>().endNode;
                            transition = null;
                        }
                                //check if destination node != current Node grey transition
                                //Update current Node
                        
                                //grey current Node
                                //if failed situation put passed to false, processed to true, eating to false
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
        mealCount = meal.Count;
        currentBrochette = meal[0];
        meal.Remove(currentBrochette);
        currentAlimentIndex = currentBrochette.transform.childCount -1;
        processed = false;
        currentNode = mouthNode;
    }


    public void Reset()
    {

    }

}
