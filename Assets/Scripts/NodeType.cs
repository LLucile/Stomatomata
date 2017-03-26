using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeType : MonoBehaviour {
    public enum tType
    {
        Red,
        NotRed,
        Green,
        NotGreen,
        Blue,
        NotBlue,
        All,
        Stomach,
        Alien
    };

    public tType type = tType.Red;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
