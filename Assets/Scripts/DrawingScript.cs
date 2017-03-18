using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingScript : MonoBehaviour
{

    public GameObject objectOver;
    public GameObject currentTrans;
    public GameObject nodePrefab;

    public float drawingCanvasMinX = -5f;
    public float drawingCanvasMaxX = 5f;
    public float drawingCanvasMinY = -5f;
    public float drawingCanvasMaxY = 5f;

    public bool onDrawingCanvas = false;

    private GameObject beginNode;
    private GameObject endNode;



    //script variables
    private bool mouseDown;
    private bool mouseRightDown = false;
    private bool previousmouseDown = false;
    private bool previousmouseRightDown = false;
    private bool mouseReleased = true;
    private float angle = 0f;
    private Vector3 currentMousePosDelta;
    private bool startedNewTrans = false;
    private float mouseDistance = 0f;

    private bool newNode = false;
    private GameObject myNewNode;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - Camera.main.transform.position.z));
        mouseDown = Input.GetMouseButton(0);
        mouseRightDown = Input.GetMouseButton(1);

        //Test if is onDrawingCanvas
        if (mousePosition.x < drawingCanvasMaxX && mousePosition.x > drawingCanvasMinX && mousePosition.y < drawingCanvasMaxY && mousePosition.y > drawingCanvasMinY)
        {
            onDrawingCanvas = true;
        }
        else
        {
            onDrawingCanvas = false;
        }
        if (onDrawingCanvas)
        {
            if (!newNode)
            {
                //Instantiate newNode
                newNode = true;
                myNewNode = Instantiate(nodePrefab, mousePosition, Quaternion.identity);
            }
            if (myNewNode != null && this.objectOver == null)
            {
                    myNewNode.GetComponent<Transform>().position = mousePosition;
            }
        }
        else
        {
            GameObject.Destroy(myNewNode);
            newNode = false;
        }
        if (previousmouseRightDown != mouseRightDown)
        {
            Debug.Log("HERE IS A RIGHT CLICK !!!!!!!!!!!!!!!!!!");
            if (mouseRightDown)
            {
                GameObject.Destroy(objectOver);
            }
        }
        if (previousmouseDown != mouseDown)
        {
            //Debug.Log(mouseDown);
            if (mouseDown == true)
            {
                Debug.Log("hey mouse is down !");
                
                //Debug.Log("it is over " + this.objectOver.name);
                if (objectOver != null)
                {
                    Debug.Log("It clicked " + objectOver.name);
                    if (objectOver.CompareTag("Node"))
                    {
                        beginNode = this.objectOver;
                        beginNode.GetComponent<Node>().CreateTrans(beginNode.transform.position);
                        startedNewTrans = true;
                    }
                    else if (this.objectOver != null && this.objectOver.CompareTag("TransitionChange"))
                    {
                        this.objectOver.GetComponent<TypeTransition>().ChangeState();
                    }
                }
                else
                {
                    Debug.Log("Instantiating new Node");
                    //Activate new Node script and collider
                    newNode = false;
                    myNewNode.GetComponent<Node>().enabled = true;
                    myNewNode.GetComponent<CircleCollider2D>().enabled = true;
                    myNewNode = null;
                }
            }
            else
            {
                //if (currentTrans != null) Debug.Log(this.currentTrans.name);
                //Debug.Log("mouse is Up !");
                if (this.currentTrans != null)
                {
                    //Debug.Log("MA TRANSITION EXIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIISTE");
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
        }
        if (mouseDown && startedNewTrans)
        {
            currentMousePosDelta = mousePosition - beginNode.transform.position;
            //lets compute angle
            currentTrans.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePosition.y - beginNode.transform.position.y), (mousePosition.x - beginNode.transform.position.x)) * Mathf.Rad2Deg);
            //Debug.Log("hey check that nice angle transition has ! Angle = " + currentTrans.transform.localEulerAngles.z);
            //lets compute distance
            mouseDistance = Mathf.Sqrt(currentMousePosDelta.x * currentMousePosDelta.x + currentMousePosDelta.y * currentMousePosDelta.y);
            //Debug.Log("hey check that nice distance mouse has ! Distance = " + mouseDistance);
            if (mouseDistance > 1)
            {
                // TO DO : here we will need to add the sprite change for scale changes
                currentTrans.transform.localScale = new Vector3(mouseDistance / 2, mouseDistance / 2, currentTrans.transform.localScale.z);
            }
            else
            {
                //TO DO : here use self-node transition sprite
            }
        }
        previousmouseDown = mouseDown;
        previousmouseRightDown = mouseRightDown;

    }

}
