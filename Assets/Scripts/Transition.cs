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
        All
    };
    public GameObject endNode;
    public GameObject startNode;
    public tType transitionType;

    public bool eat(string skewer) {
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
}
