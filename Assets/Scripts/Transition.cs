using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour {
    public enum tType {
        Red,
        Green,
        Blue,
        NotRed,
        NotGreen,
        NotBlue,
        Stomach,
        All
    };
    public GameObject endNode;
    public GameObject startNode;
    public tType transitionType;

    private bool onmouseOver = false;
    public DrawingScript gameManager;

    // Use this for initialization
    void Start()
    {
        this.gameManager = GameObject.FindWithTag("GameManager").GetComponent<DrawingScript>();
    }

    public bool eat(string skewer) {
        if (transitionType == tType.Stomach) 
        {
            if (skewer[skewer.Length - 1] == 'o')
            {
                return true;
            }
            return false;
        }
        if (skewer[skewer.Length - 1] == 'r') {
            if (transitionType == tType.Green || transitionType == tType.Blue || transitionType == tType.NotRed) {
                return false;
            }
        } else if (skewer[skewer.Length - 1] == 'g') {
            if (transitionType == tType.Red || transitionType == tType.Blue || transitionType == tType.NotGreen) {
                return false;
            }
        } else {
            if (transitionType == tType.Red || transitionType == tType.Green || transitionType == tType.NotBlue) {
                return false;
            }
        }
        Node nextNode = endNode.GetComponent<Node>();
        skewer = skewer.Substring(0, skewer.Length - 2);
        for (int i = 0; i < nextNode.transitions.Count; ++i) {
            if (nextNode.transitions[i].GetComponent<Transition>().eat(skewer)) {
                return true;
            }
        }
        return false;
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
