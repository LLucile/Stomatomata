using System.Collections;
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
        Color tempcolor =  this.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Vector4(tempcolor[0], tempcolor[1], tempcolor[2], 1.0f);
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void CreateTrans(Vector3 position)
    {
        //Debug.Log(this.gameObject.name + " said : I am going to instanciate something !");
        GameObject newTrans = Instantiate(transitionsPrefab, position, Quaternion.identity);
        //Debug.Log(this.gameObject.name + " said : Instantiated " + newTrans.name);
        newTrans.GetComponent<Transition>().startNode = this.gameObject;
        this.transitions.Add(newTrans);
        this.gameManager.currentTrans = newTrans;
    }


    void OnDestroy()
    {
        for (int i = 0; i < transitions.Count; i++)
        {
            GameObject.Destroy(transitions[i]);
        }
    }


    void OnMouseExit()
    {
        this.onmouseOver = false;
        gameManager.objectOver = null;
    }

    void OnMouseEnter()
    {
        this.onmouseOver = true;
        gameManager.objectOver = this.gameObject;
    }











}
