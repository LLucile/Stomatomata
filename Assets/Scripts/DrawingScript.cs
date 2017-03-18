using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingScript : MonoBehaviour {

    public GameObject objectOver;
    public GameObject currentTrans;

    private GameObject beginNode;
    private GameObject endNode;
    



    //script variables
    bool mouseDown;
    private bool previousmouseDown = false;
    private bool mouseReleased = true;
    private float angle = 0f;
    private Vector3 currentMousePosDelta;
    private bool startedNewTrans = false;
    private float mouseDistance = 0f;

    // Use this for initialization
    void Start()
    {

    }

    void Update () {
        mouseDown = Input.GetMouseButton(0);

        if (previousmouseDown != mouseDown )
        {
            if(mouseDown == true){
                Debug.Log("hey mouse is down !");
                beginNode = this.objectOver;
                if(beginNode != null){
                    Debug.Log("It clicked " + beginNode.name);
                    if (beginNode.CompareTag("Node"))
                    {
                        beginNode.GetComponent<Node>().CreateTrans(beginNode.transform.position);
                        startedNewTrans = true;
                    }
                    else
                    {
                        Debug.Log("sorry but this object does not qualify as a beginning node");
                    }
                }
            }
            else
            {
                Debug.Log("hey mouse is up !");
                beginNode = null;
                startedNewTrans = false;
                if (this.objectOver == null)
                {
                    Debug.Log("Sorry but this is not a valid end object");
                    this.currentTrans.GetComponent<Transition>().startNode.GetComponent<Node>().transitions.Remove(this.currentTrans);
                    Destroy(this.currentTrans);
                }
                else
                {
                    //TODO snap on the edge of gameObject instead as anywhere
                    this.endNode = this.objectOver;
                    this.currentTrans.GetComponent<Transition>().endNode = this.endNode;
                }
            }
        }
        if (mouseDown && startedNewTrans)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));
            currentMousePosDelta = mousePosition - beginNode.transform.position;
            //lets compute angle
            currentTrans.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePosition.y - beginNode.transform.position.y), (mousePosition.x - beginNode.transform.position.x)) * Mathf.Rad2Deg);
            Debug.Log("hey check that nice angle transition has ! Angle = " + currentTrans.transform.localEulerAngles.z);  
            //lets compute distance
            mouseDistance = Mathf.Sqrt(currentMousePosDelta.x*currentMousePosDelta.x + currentMousePosDelta.y*currentMousePosDelta.y);
            Debug.Log("hey check that nice distance mouse has ! Distance = " + mouseDistance);
            if (mouseDistance > 1)
            {
                // TO DO : here we will need to add the sprite change for scale changes
                currentTrans.transform.localScale = new Vector3(mouseDistance, mouseDistance, currentTrans.transform.localScale.z);
            }
            else
            {
                //TO DO : here use self-node transition sprite
            }
        }
        previousmouseDown = mouseDown;
	}

}
