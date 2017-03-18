using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ParseLevels : MonoBehaviour {
    public struct LevelStruct {
        public int levelNumber;
        public List<string> skewerList;
        public string description;
    }
    [HideInInspector]
    public List<LevelStruct> levels;
    public List<GameObject> levelsGameObjects;
    public GameObject skewerPrefab;
    void Start() {
        levels = new List<LevelStruct>();
        levelsGameObjects = new List<GameObject>();
        ReadLevels();
        /*for (int i = 0; i < levels.Count; ++i) {
            LevelStruct lvl = getLevel(i);
            Debug.Log(lvl.levelNumber);
            foreach (string skewer in lvl.skewerList) {
                Debug.Log(skewer);
            }
            Debug.Log(lvl.description);
        }*/
    }
    public LevelStruct getLevel(int levelNb) {
        if (levels.Count > levelNb) {
            return levels[levelNb];
        }
        Debug.Log("Level was not loaded");
        return levels[0];
    }
    public void createLevelGameObject(LevelStruct level) {
        GameObject obj = new GameObject();
        obj.name = level.levelNumber.ToString();
        Vector3 pos = new Vector3(2.0f, 2.0f, .0f);
        float step = .8f;
        for (int i = 0; i < level.skewerList.Count; ++i) {
            GameObject metaStick = new GameObject();
            metaStick.transform.SetParent(obj.transform);
            metaStick.name = "metaStick";
            GameObject stick = Instantiate(skewerPrefab);
            stick.name = "stick";
            stick.transform.SetParent(metaStick.transform);
            stick.transform.localPosition = pos + new Vector3(step * ((level.skewerList[i].Length - 4.0f) / 2.0f), .0f, .0f);
            stick.transform.localScale = new Vector3(level.skewerList[i].Length + 2.0f, .5f, 1.0f);
            stick.GetComponent<SpriteRenderer>().color = Color.black;
            stick.GetComponent<SpriteRenderer>().sortingOrder = 4;
            GameObject eatable = Instantiate(skewerPrefab);
            eatable.name = "isEatable";
            eatable.transform.SetParent(metaStick.transform);
            eatable.transform.localPosition = pos - new Vector3(step, .0f, .0f);
            if (level.skewerList[i][0] == 'x') {
                eatable.GetComponent<SpriteRenderer>().color = Color.magenta;
            } else {
                eatable.GetComponent<SpriteRenderer>().color = Color.white;
            }
            for (int j = 1; j < level.skewerList[i].Length; ++j) {
                GameObject skewElem = Instantiate(skewerPrefab);
                skewElem.name = "nomnomnom";
                if (level.skewerList[i][j] == 'r') {
                    skewElem.GetComponent<SpriteRenderer>().color = Color.red;
                } else if (level.skewerList[i][j] == 'g') {
                    skewElem.GetComponent<SpriteRenderer>().color = Color.green;
                } else {
                    skewElem.GetComponent<SpriteRenderer>().color = Color.blue;
                }
                skewElem.transform.SetParent(metaStick.transform);
                skewElem.transform.localPosition = pos;
                pos.x += step;
            }
            pos.y += step;
            pos.x = 2.0f;
        }
        levelsGameObjects.Insert(level.levelNumber - 1, obj);
        if (level.levelNumber != 1) {
            obj.SetActive(false);
        }
    }
    public void ReadLevels() {
        StreamReader inputStream = new StreamReader("Assets/Scenes/levels.txt");
        while (!inputStream.EndOfStream) {
            LevelStruct levelRead = new LevelStruct();
            string inputLine = inputStream.ReadLine();
            while ((inputLine == "" || inputLine == "\n") && !inputStream.EndOfStream) {
                inputLine = inputStream.ReadLine();
            }
            levelRead.levelNumber = int.Parse(inputLine);
            levelRead.skewerList = new List<string>();
            inputLine = inputStream.ReadLine();
            while ((inputLine[0] == 'o' || inputLine[0] == 'x') && !inputStream.EndOfStream) {
                levelRead.skewerList.Add(inputLine);
                inputLine = inputStream.ReadLine();
            }
            levelRead.description = inputLine;
            levels.Insert(levelRead.levelNumber - 1, levelRead);
            createLevelGameObject(levelRead);
        }
        inputStream.Close();
    }
}
