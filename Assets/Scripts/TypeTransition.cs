using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeTransition : MonoBehaviour {


    private bool onmouseOver = false;
    public DrawingScript gameManager;

	// Use this for initialization
    void Start()
    {
        this.gameManager = GameObject.FindWithTag("GameManager").GetComponent<DrawingScript>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeState(int neg)
    {
        Debug.Log("Y'a pas de code, mais j'ai capté qu'il faut changer de state !");
        //Change le type du parent
        if (neg == 0)
        {
            this.transform.parent.GetComponentInParent<Transition>().NegState();
        }
        else if(neg > 0)
        {
            this.transform.parent.GetComponentInParent<Transition>().NextState();
        }
        else if (neg == -1)
        {

        }
    }

    void OnMouseEnter()
    {
        this.onmouseOver = true;
        gameManager.objectOver = this.gameObject;
        //Debug.Log("you're over " + this.gameObject.name);
    }

    void OnMouseExit()
    {
        this.onmouseOver = false;
        gameManager.objectOver = null;
        //Debug.Log("you're leaving " + this.gameObject.name);
    }
}
