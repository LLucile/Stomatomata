using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour {
    public enum tType {
        Red,
        NotRed,
        Green,
        NotGreen,
        Blue,
        NotBlue,
        All,
        Stomach,        
    };
    public GameObject endNode;
    public GameObject startNode;
    public tType transitionType;

    private tType pTransitionType;

    private bool onmouseOver = false;
    public DrawingScript gameManager;

    private Transform tpanel;


    // Use this for initialization
    void Start()
    {
        this.gameManager = GameObject.FindWithTag("GameManager").GetComponent<DrawingScript>();
        transitionType = tType.All;
    }

    void Update()
    {
        if (transitionType != pTransitionType)
        {
            //CHANGER L'AFFICHAGE
                //Find active child
            for (int i = 0; i < this.transform.childCount; ++i)
            {
                if(transform.GetChild(i).gameObject.activeSelf) tpanel = transform.GetChild(i).FindChild("Panneau");
                //Find its TypeTransitionObject
                if (transitionType < tType.All)
                {
                    SetBack((int)transitionType % 2);
                    SetIcon(transitionType);
                }
                else if (transitionType == tType.All)
                {
                    SetBack(0);
                    SetIcon(tType.All);
                }               
            }                
        }
        pTransitionType = transitionType;
    }

    bool isResultWanted(char first, bool result) {
        if ((first == 'o' && result) || (first == 'x' && !result))
            return true;
        return false;
    }

    private void SetBack(int yesno){
        if (yesno == 1)
        {
            //Set NO background
           // Debug.Log("black back");
            tpanel.FindChild("Fond").FindChild("FondNon").gameObject.SetActive(true);
            tpanel.FindChild("Fond").FindChild("FondOui").gameObject.SetActive(false);

        }
        else
        {
            //Set YES background
           // Debug.Log("white back");
            tpanel.FindChild("Fond").FindChild("FondOui").gameObject.SetActive(true);
            tpanel.FindChild("Fond").FindChild("FondNon").gameObject.SetActive(false);

        }
    }

    private void SetIcon(tType iconType){
        switch(iconType){
            case tType.Red : 
            case tType.NotRed:
            //    Debug.Log("red icon");
                tpanel.FindChild("Item").FindChild("Red").gameObject.SetActive(true);
                tpanel.FindChild("Item").FindChild("Green").gameObject.SetActive(false);
                tpanel.FindChild("Item").FindChild("Blue").gameObject.SetActive(false);
                break;
            case tType.Green:
            case tType.NotGreen:
            //    Debug.Log("green icon");
                tpanel.FindChild("Item").FindChild("Red").gameObject.SetActive(false);
                tpanel.FindChild("Item").FindChild("Green").gameObject.SetActive(true);
                tpanel.FindChild("Item").FindChild("Blue").gameObject.SetActive(false);
                break;
            case tType.Blue:
            case tType.NotBlue:
            //    Debug.Log("blue icon");
                tpanel.FindChild("Item").FindChild("Red").gameObject.SetActive(false);
                tpanel.FindChild("Item").FindChild("Green").gameObject.SetActive(false);
                tpanel.FindChild("Item").FindChild("Blue").gameObject.SetActive(true);
                break;
            default :
            //    Debug.Log("no icon");
                tpanel.FindChild("Item").FindChild("Red").gameObject.SetActive(false);
                tpanel.FindChild("Item").FindChild("Green").gameObject.SetActive(false);
                tpanel.FindChild("Item").FindChild("Blue").gameObject.SetActive(false);
                break;
        }
    }

    public bool eat(string skewer) {
        if (transitionType == tType.Stomach) {
            return isResultWanted(skewer[skewer.Length - 1], true);
        }
        //Debug.Log("------------------");
        //Debug.Log(skewer);
        if (skewer[skewer.Length - 1] == 'r') {
            //Debug.Log("r");
            if (transitionType == tType.Green || transitionType == tType.Blue || transitionType == tType.NotRed) {
                //Debug.Log("return");
                return isResultWanted(skewer[0], false);
            }
        } else if (skewer[skewer.Length - 1] == 'g') {
            //Debug.Log("g");
            if (transitionType == tType.Red || transitionType == tType.Blue || transitionType == tType.NotGreen) {
                return isResultWanted(skewer[0], false);
            }
        } else if (skewer[skewer.Length - 1] == 'b') {
            //Debug.Log("b");
            if (transitionType == tType.Red || transitionType == tType.Green || transitionType == tType.NotBlue) {
                return isResultWanted(skewer[0], false);
            }
        } else {
            Debug.Log("stomach not found");
            return isResultWanted(skewer[0], false);
        }
        Node nextNode = endNode.GetComponent<Node>();
        skewer = skewer.Substring(0, skewer.Length - 1);
        //Debug.Log(nextNode.name);
        //Debug.Log(skewer);
        //Debug.Log(nextNode.transitions.Count);
        for (int i = 0; i < nextNode.transitions.Count; ++i) {
            //Debug.Log(nextNode.transitions[i].name);
            //Debug.Log(skewer);
            if (nextNode.transitions[i].GetComponent<Transition>().eat(skewer)) {
                return true;
            }
        }
        Debug.Log("non specified, indigestion");
        return isResultWanted(skewer[0], false);
    }

    public void NextState()
    {
        Debug.Log("Changing state from " + transitionType);
        transitionType+=2;
        if (transitionType > tType.All)
        {
            if ((int)transitionType % 2 == 0) transitionType = 0;
            else transitionType = tType.NotRed;
        }
        Debug.Log(" to " + transitionType);
    }

    public void NegState()
    {
        if (transitionType != tType.All)
        {
            if (((int)transitionType % 2) == 0)
            {
                transitionType++;
            }
            else
            {
                transitionType--;
            }
        }
    }

    public void StomachState()
    {
        transitionType = tType.Stomach;
    }

}
