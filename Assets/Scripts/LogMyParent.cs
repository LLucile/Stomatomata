using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMyParent : MonoBehaviour {

    private bool onmouseOver = false;
    public DrawingScript gameManager;

    // Use this for initialization
    void Start()
    {
        this.gameManager = GameObject.FindWithTag("GameManager").GetComponent<DrawingScript>();
    }

    void OnMouseExit()
    {
        this.onmouseOver = false;
        if (gameManager.objectOver.name == this.transform.parent.name)
        {
            gameManager.objectOver = null;
        }
        //Debug.Log("you're leaving " + this.transform.parent.gameObject.name);
    }

    void OnMouseEnter()
    {
        this.onmouseOver = true;
        gameManager.objectOver = this.transform.parent.gameObject;
        //Debug.Log("you're over " + this.transform.parent.gameObject.name);
    }
}
