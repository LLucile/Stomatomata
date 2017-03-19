using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGame : MonoBehaviour {
    static int currentLevel = 0;
    ParseLevels parser;
    void Start() {
        parser = GameObject.FindObjectOfType<ParseLevels>();
    }

    public void tryToEat() {
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

    public void ResetLevel() {
        Debug.Log("TODO: clear transitions & nodes");
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
