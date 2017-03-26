using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageGame : MonoBehaviour {

    public GameObject panel;
    static int currentLevel = 0;

    private int pLevel = 0;

    public Text greet;
    public Text levelOver;
    public Text brochetteFailedOrPassed;

    Text rules;
    ParseLevels parser;

    private float timerLevelDisplay = 180;
    private bool displayLevel = false;

    private float timerResultDisplay = 180;
    private bool resultDisplay = false;

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

    void Update()
    {
        //change resultDisplay for Feeder.processed
        if (resultDisplay)
        {
            timerResultDisplay--;
            if (timerResultDisplay < 0)
            {
                timerResultDisplay = 180;
                resultDisplay = false;
                brochetteFailedOrPassed.gameObject.SetActive(false);
            }
        }
        if (currentLevel != pLevel){
            displayLevel = true;
        }
        if(displayLevel)
        {
            if(!greet.gameObject.activeSelf) greet.gameObject.SetActive(true);
            greet.text = "Level " + currentLevel;
            timerLevelDisplay--;
        }
        if (timerLevelDisplay < 0)
        {
            displayLevel = false;
            greet.gameObject.SetActive(false);
            timerLevelDisplay = 180;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!panel.activeSelf)
            {
                Time.timeScale = 0.000001f;
                panel.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                panel.SetActive(false);
            }
        }
        pLevel = currentLevel;
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

    /*public void tryToEat() {
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
                StartCoroutine(node.transitions[j].GetComponent<Transition>().eat(level.skewerList[i],(myReturnValue) => {
                    if(myReturnValue) { //Debug.Log(level.skewerList[i]); 
                        isSkewDone = true;
                    }
                 }));
            }
            if (!isSkewDone) {
                brochetteFailedOrPassed.gameObject.SetActive(true);
                brochetteFailedOrPassed.transform.parent.gameObject.SetActive(true);
                brochetteFailedOrPassed.text += " \n Brochette " + i + " failed the test";
                resultDisplay = true;
                Debug.Log("Level failed");
                Destroy(bulle);
                return;
            }
            //Debug.Log("destroy brochette");
            Destroy(brochette);
        }
        Debug.Log("destroy bulle");
        Destroy(bulle);
        if (isSkewDone) {
            GoToNextLevel();
        }
    }*/

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
