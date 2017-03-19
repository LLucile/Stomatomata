using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeSetter : MonoBehaviour {

    private bool zoomed = false;

    public float cameraSizeZoomed = 6f;

    public void SetSize()
    {
        ManageGame.CleanupUnecessaryNode();
        if (zoomed)
        {
            Camera.main.orthographicSize = cameraSizeZoomed;
        }
        else
        {
            Camera.main.orthographicSize = 7.68f;
        }
        zoomed = !zoomed;
    }
	
}
