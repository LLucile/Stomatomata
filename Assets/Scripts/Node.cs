﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public DrawingScript gameManager;
    public List<GameObject> transitions;
    public GameObject transitionsPrefab;

    private bool onmouseOver = false;

    

	// Use this for initialization
	void Start () {
		this.gameManager = GameObject.FindWithTag("GameManager").GetComponent<DrawingScript>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void CreateTrans(Vector3 position)
    {
        Debug.Log(this.gameObject.name + " said : I am going to instanciate something !");
        GameObject newTrans = Instantiate(transitionsPrefab, position, Quaternion.identity);
        Debug.Log(this.gameObject.name + " said : Instantiated " + newTrans.name);
        this.transitions.Add(newTrans);
        this.gameManager.currentTrans = newTrans;
    }


    void OnMouseExit()
    {
        this.onmouseOver = false;
        gameManager.objectOver = null;
        Debug.Log("you're leaving " + this.gameObject.name);
    }

    void OnMouseEnter()
    {
        this.onmouseOver = true;
        gameManager.objectOver = this.gameObject;
        Debug.Log("you're over " + this.gameObject.name);
    }











}
