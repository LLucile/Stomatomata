using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageGame : MonoBehaviour {
    static int currentLevel = 0;
    Text rules;
    ParseLevels parser;
    public GameObject BullePrefab;
    void Start() {
        parser = GameObject.FindObjectOfType<ParseLevels>();
        Text[] texts = GameObject.FindObjectsOfType<Text>();
        for (int i = 0; i < texts.Length; ++i) {
            if (texts[i].name == "Rules") {
                rules = texts[i];
                return;
            }
        }
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
        GameObject bulle = Instantiate(BullePrefab);
        bool isSkewDone = false;
        for (int i = 0; i < level.skewerList.Count; ++i) {
            GameObject brochette = Instantiate(parser.levelsGameObjects[i]);
            brochette.transform.parent = bulle.transform;
            isSkewDone = false;
            //Debug.Log(i);
            for (int j = 0; j < node.transitions.Count; ++j) {
                if (node.transitions[j].GetComponent<Transition>().eat(level.skewerList[i])) {
                    Debug.Log(level.skewerList[i]);
                    isSkewDone = true;
                    continue;
                }
            }
            if (!isSkewDone) {
                Debug.Log("Level failed");
                Destroy(bulle);
                return;
            }
            Debug.Log("destroy brochette");
            Destroy(brochette);
        }
        Debug.Log("destroy bulle");
        Destroy(bulle);
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
            } else if (nodes[i].name != "EstomacFinal") {
                clearList(nodes[i].transitions);
                Destroy(nodes[i].gameObject);
            }
        }
    }

    public void GoToPreviousLevel() {
        if (currentLevel > 0) {
            parser.levelsGameObjects[currentLevel].SetActive(false);
            --currentLevel;
            parser.levelsGameObjects[currentLevel].SetActive(true);
            rules.text = parser.levels[currentLevel].description;
            ResetLevel();
        } else {
            Debug.Log("You are already at the first level");
        }
    }

    public void GoToNextLevel() {
        if (parser.levels.Count > currentLevel + 1) {
            parser.levelsGameObjects[currentLevel].SetActive(false);
            ++currentLevel;
            parser.levelsGameObjects[currentLevel].SetActive(true);
            rules.text = parser.levels[currentLevel].description;
            ResetLevel();
        } else {
            Debug.Log("You reached the end");
        }
    }

    public void Quit() {
        Application.Quit();
    }
}
