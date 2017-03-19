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
    [HideInInspector]
    public List<GameObject> levelsGameObjects;
    public Sprite redPrefab;
    public Sprite bluePrefab;
    public Sprite greenPrefab;
    public Sprite humanPrefab;
    public Sprite alienPrefab;
    public Sprite pikPrefab;
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
        float step = 1.0f;
        for (int i = 0; i < level.skewerList.Count; ++i) {
            GameObject metaStick = new GameObject();
            metaStick.transform.SetParent(obj.transform);
            metaStick.name = "metaStick";
            GameObject stick = new GameObject();
            stick.AddComponent<SpriteRenderer>();
            stick.name = "stick";
            stick.transform.SetParent(metaStick.transform);
            stick.transform.localPosition = pos + new Vector3(step * ((level.skewerList[i].Length - 4.0f) / 2.0f), .0f, .0f);
            stick.transform.localScale = new Vector3(level.skewerList[i].Length, .5f, 1.0f);
            stick.GetComponent<SpriteRenderer>().sprite = pikPrefab;
            stick.GetComponent<SpriteRenderer>().sortingOrder = 4;
            GameObject eatable = new GameObject();
            eatable.AddComponent<SpriteRenderer>();
            eatable.GetComponent<SpriteRenderer>().sortingOrder = 5;
            eatable.name = "isEatable";
            eatable.transform.SetParent(metaStick.transform);
            eatable.transform.localPosition = pos - new Vector3(step, .0f, .0f);
            if (level.skewerList[i][0] == 'x') {
                eatable.GetComponent<SpriteRenderer>().sprite = alienPrefab;
            } else {
                eatable.GetComponent<SpriteRenderer>().sprite = humanPrefab;
            }
            for (int j = 1; j < level.skewerList[i].Length; ++j) {
                GameObject skewElem = new GameObject();
                skewElem.AddComponent<SpriteRenderer>();
                skewElem.GetComponent<SpriteRenderer>().sortingOrder = 5;
                skewElem.name = "nomnomnom";
                if (level.skewerList[i][j] == 'r') {
                    skewElem.GetComponent<SpriteRenderer>().sprite = redPrefab;
                } else if (level.skewerList[i][j] == 'g') {
                    skewElem.GetComponent<SpriteRenderer>().sprite = greenPrefab;
                } else {
                    skewElem.GetComponent<SpriteRenderer>().sprite = bluePrefab;
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
