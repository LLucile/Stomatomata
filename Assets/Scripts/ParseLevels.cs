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
    public List<LevelStruct> levels;
    void Start() {
        levels = new List<LevelStruct>();
        ReadLevels();
        for (int i = 0; i < levels.Count; ++i) {
            LevelStruct lvl = getLevel(i);
            Debug.Log(lvl.levelNumber);
            foreach (string skewer in lvl.skewerList) {
                Debug.Log(skewer);
            }
            Debug.Log(lvl.description);
        }
    }
    public LevelStruct getLevel(int levelNb) {
        if (levels.Count > levelNb) {
            return levels[levelNb];
        }
        Debug.Log("Level was not loaded");
        return levels[0];
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
            levels.Add(levelRead);
        }
        inputStream.Close();
    }
}
