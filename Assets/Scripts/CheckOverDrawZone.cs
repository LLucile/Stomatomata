using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOverDrawZone : MonoBehaviour {

    public DrawingScript gameManager;

    private bool onmouseOver = false;

	// Use this for initialization
    void Start()
    {
        this.gameManager = GameObject.FindWithTag("GameManager").GetComponent<DrawingScript>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseExit()
    {
        this.gameManager.onDrawingCanvas = false;
        Debug.Log("you're leaving " + this.gameObject.name);
    }

    void OnMouseEnter()
    {
        this.gameManager.onDrawingCanvas = true;
        Debug.Log("you're over " + this.gameObject.name);
    }
}
