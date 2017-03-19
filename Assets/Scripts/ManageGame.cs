using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGame : MonoBehaviour {
    static int currentLevel = 0;
    ParseLevels parser;
    void Start() {
        parser = GameObject.FindObjectOfType<ParseLevels>();
    }

    public static void CleanupUnecessaryNode() {
        Vector3 mousePos = Input.mousePosition;
        Vector2 v = Camera.main.ScreenToWorldPoint(mousePos);
        Collider2D[] col = Physics2D.OverlapPointAll(v);
        if (col.Length > 0) {
            foreach (Collider2D c in col) {
                if (c.CompareTag("Node")) {
                    Destroy(c.gameObject);
                }
            }
        }
    }

    public void tryToEat() {
        CleanupUnecessaryNode();
        ParseLevels.LevelStruct level = parser.getLevel(currentLevel);
        Node node = GameObject.Find("FirstNode").GetComponent<Node>();
        bool isSkewDone = false;
        for (int i = 0; i < level.skewerList.Count; ++i) {
            isSkewDone = false;
            Debug.Log(i);
            for (int j = 0; j < node.transitions.Count; ++j) {
                if (node.transitions[j].GetComponent<Transition>().eat(level.skewerList[i])) {
                    Debug.Log(level.skewerList[i]);
                    isSkewDone = true;
                    continue;
                }
            }
            if (!isSkewDone) {
                Debug.Log("Level failed");
                return;
            }
        }
        if (isSkewDone) {
            GoToNextLevel();
        }
    }

    void clearList(List<GameObject> obj) {
        for (int i = 0; i < obj.Count; ++i) {
            Destroy(obj[i]);
        }
    }

    public void ResetLevel() {
        Node[] nodes = GameObject.FindObjectsOfType<Node>();
        for (int i = 0; i < nodes.Length; ++i) {
            if (nodes[i].name == "FirstNode") {
                clearList(nodes[i].transitions);
            } else if (nodes[i].name != "FinalNode" && nodes[i].name != "EstomacFinal") {
                clearList(nodes[i].transitions);
                Destroy(nodes[i].gameObject);
            }
        }
    }

    public void GoToNextLevel() {
        if (parser.levels.Count > currentLevel + 1) {
            parser.levelsGameObjects[currentLevel].SetActive(false);
            ++currentLevel;
            parser.levelsGameObjects[currentLevel].SetActive(true);
            ResetLevel();
            Debug.Log("Level completed !");
        } else {
            Debug.Log("You reached the end");
        }
    }
}
