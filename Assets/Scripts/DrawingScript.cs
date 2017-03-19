using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingScript : MonoBehaviour
{
    public GameObject objectOver;
    public GameObject currentTrans;
    public GameObject nodePrefab;
    public GameObject bouclePrefab;

    public float mouseDistanceMin = 0.4f;
    public float mouseDistanceMinLong = 2.0f;

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
    private bool longSprite = false;

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
            //Debug.Log("HERE IS A RIGHT CLICK !!!!!!!!!!!!!!!!!!");
            if (mouseRightDown)
            {
                Debug.Log("Hey mouse is going DOWN !!!!!!!!!!!");
                if (objectOver != null && objectOver.CompareTag("TransitionChange"))
                {
                    this.objectOver.GetComponent<TypeTransition>().ChangeState(0);
                }
                else if (objectOver.name == "FirstNode" || objectOver.name == "EstomacFinal")
                {
                    Debug.Log("You cannot destroy that !!!!!!!!!!!!!!!");
                }
                else
                {
                    GameObject.Destroy(objectOver);
                }
            }
        }
        if (previousmouseDown != mouseDown)
        {
            //Debug.Log(mouseDown);
            if (mouseDown == true)
            {
                //Debug.Log("hey mouse is down !");
                
                //Debug.Log("it is over " + this.objectOver.name);
                if (objectOver != null && objectOver.name != "EstomacFinal")
                {
                    //Debug.Log("It clicked " + objectOver.name);
                    if (objectOver.CompareTag("Node"))
                    {
                        beginNode = this.objectOver;
                        beginNode.GetComponent<Node>().CreateTrans(beginNode.transform.position);
                        startedNewTrans = true;
                    }
                    else if (this.objectOver != null && this.objectOver.CompareTag("TransitionChange"))
                    {
                        Debug.Log("HEY ! Let's change transition state !");
                        this.objectOver.GetComponent<TypeTransition>().ChangeState(1);
                    }
                }
                else
                {
                    //Debug.Log("Instantiating new Node");
                    //Activate new Node script and collider
                    newNode = false;
                    if (myNewNode != null)
                    {
                        myNewNode.GetComponent<Node>().enabled = true;
                        myNewNode.GetComponent<CircleCollider2D>().enabled = true;
                    }
                    myNewNode = null;
                }
            }
            else
            {
                //Debug.Log("mouse is Up !");
                if (this.currentTrans != null)
                {
                    beginNode = null;
                    startedNewTrans = false;

                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = 5f;

                    Vector2 v = Camera.main.ScreenToWorldPoint(mousePos);

                    Collider2D[] col = Physics2D.OverlapPointAll(v); //use raycast to drop collisions with transitions

                    bool done = false;
                    if (col.Length > 0)
                    {
                        foreach (Collider2D c in col)
                        {
                            //Debug.Log(c.gameObject.name);
                            if (c.gameObject.CompareTag("Node")) {
                                this.endNode = c.gameObject;
                                Transition t = currentTrans.GetComponent<Transition>();
                                t.endNode = this.endNode;
                                float dist = Vector3.Distance(t.startNode.transform.position, t.endNode.transform.position);
                                //change sprite for stomach sprite if endNode == "EstomacFinal"
                                if (endNode.name == "EstomacFinal")
                                {
                                    //Debug.Log("Snapping on Stomach !!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                    currentTrans.transform.localScale = new Vector3(dist / 2, (currentTrans.transform.localScale.y / Mathf.Abs(currentTrans.transform.localScale.y)) * dist / 2, currentTrans.transform.localScale.z); //snap (middle of node)
                                    if (longSprite)
                                    {
                                        SetSprite(currentTrans, "stomachLong");
                                        currentTrans.GetComponent<Transition>().transitionKindName = "stomachLong";
                                        currentTrans.transform.localScale = new Vector3(dist / 4, (currentTrans.transform.localScale.y / Mathf.Abs(currentTrans.transform.localScale.y)) * dist / 4, currentTrans.transform.localScale.z); //snap (middle of node)
                                    }
                                    else
                                    {
                                        SetSprite(currentTrans, "stomach");
                                        currentTrans.GetComponent<Transition>().transitionKindName = "stomach";
                                    }
                                    currentTrans.GetComponentInChildren<TypeTransition>().ChangeState(-1);
                                }
                                //Debug.Log("endNode : " + this.endNode.name + " start Node : " + t.startNode);
                                if (this.endNode != t.startNode)
                                {
                                    if (longSprite)
                                    {
                                        currentTrans.transform.localScale = new Vector3(dist / 4, (currentTrans.transform.localScale.y / Mathf.Abs(currentTrans.transform.localScale.y)) * dist / 4, currentTrans.transform.localScale.z); //snap (middle of node)
                                    }
                                    else
                                    {
                                        currentTrans.transform.localScale = new Vector3(dist / 2, (currentTrans.transform.localScale.y / Mathf.Abs(currentTrans.transform.localScale.y)) * dist / 2, currentTrans.transform.localScale.z); //snap (middle of node)
                                    }
                                }
                                else
                                {
                                    currentTrans.transform.localScale = new Vector3(1, (currentTrans.transform.localScale.y / Mathf.Abs(currentTrans.transform.localScale.y)) * 1, 1); //snap (middle of node)
                                }
                                    /*if (c.gameObject == t.startNode) {
                                    GameObject boucleTrans = Instantiate(bouclePrefab);
                                    boucleTrans.GetComponent<Transition>().startNode = this.endNode;
                                    boucleTrans.GetComponent<Transition>().endNode = this.endNode;
                                    this.currentTrans.GetComponent<Transition>().startNode.GetComponent<Node>().transitions.Remove(this.currentTrans);
                                    Destroy(this.currentTrans);
                                    this.currentTrans = boucleTrans;
                                    boucleTrans.GetComponent<Transition>().startNode.GetComponent<Node>().transitions.Add(this.currentTrans);
                                    boucleTrans.transform.position = endNode.transform.position;// +new Vector3(.0f, 1.0f, .0f);
                                }*/
                                done = true;
                                this.currentTrans = null;
                                this.startedNewTrans = false;
                            }
                        }
                    }
                    if (!done)
                    {
                        this.currentTrans.GetComponent<Transition>().startNode.GetComponent<Node>().transitions.Remove(this.currentTrans);
                        Destroy(this.currentTrans);
                    }
                    /*if (this.objectOver == null || !this.objectOver.CompareTag("Node"))
                    {
                            Debug.Log("Sorry but this is not a valid end object !!! Collider transitions :C");
                            this.currentTrans.GetComponent<Transition>().startNode.GetComponent<Node>().transitions.Remove(this.currentTrans);
                            Destroy(this.currentTrans);
                    }
                    else
                    {
                        //TODO snap on the edge of gameObject instead as anywhere
                        this.endNode = this.objectOver;
                        this.currentTrans.GetComponent<Transition>().endNode = this.endNode;
                    }*/
                }
            }
        }
        if (mouseDown && startedNewTrans)
        {
            currentMousePosDelta = mousePosition - beginNode.transform.position;
            //Debug.Log("mouse distance = " + mouseDistance);
            //lets compute angle
            currentTrans.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePosition.y - beginNode.transform.position.y), (mousePosition.x - beginNode.transform.position.x)) * Mathf.Rad2Deg);
            angle = currentTrans.transform.localEulerAngles.z;
            //Debug.Log("hey check that nice angle transition has ! Angle = " + currentTrans.transform.localEulerAngles.z);
            //lets compute distance
            mouseDistance = Mathf.Sqrt(currentMousePosDelta.x * currentMousePosDelta.x + currentMousePosDelta.y * currentMousePosDelta.y);
            //Debug.Log("hey check that nice distance mouse has ! Distance = " + mouseDistance);
            if (angle > 90 && angle < 270 || angle < -90 && angle > -270)
            {
                //Debug.Log("Inversing sprite");
                currentTrans.transform.localScale = new Vector3(currentTrans.transform.localScale.x, -1 * Mathf.Abs(currentTrans.transform.localScale.y), currentTrans.transform.localScale.z);
            }
            if (mouseDistance > mouseDistanceMin)
            {
                if (mouseDistance > mouseDistanceMinLong)
                {
                    // TO DO : here we will need to add the sprite change for scale changes
                    SetSprite(currentTrans, "normalLong");
                    currentTrans.transform.localScale = new Vector3(mouseDistance / 4, (currentTrans.transform.localScale.y / Mathf.Abs(currentTrans.transform.localScale.y)) * mouseDistance / 4, currentTrans.transform.localScale.z);
                    currentTrans.GetComponent<Transition>().transitionKindName = "normalLong";
                    longSprite = true;
                }
                else
                {
                    // TO DO : here we will need to add the sprite change for scale changes
                    //Debug.Log("HI NORMAL SPRITE !");
                    SetSprite(currentTrans, "normal");
                    currentTrans.GetComponent<Transition>().transitionKindName = "normal";
                    currentTrans.transform.localScale = new Vector3(mouseDistance / 2, (currentTrans.transform.localScale.y / Mathf.Abs(currentTrans.transform.localScale.y)) * mouseDistance / 2, currentTrans.transform.localScale.z);
                    longSprite = false;
                }
            }
            else
            {
                //TO DO : here use self-node transition sprite
                SetSprite(currentTrans, "selfLoop");
                currentTrans.GetComponent<Transition>().transitionKindName = "selfLoop";
                longSprite = false;
            }
        }
        previousmouseDown = mouseDown;
        previousmouseRightDown = mouseRightDown;

    }


    void SetSprite(GameObject go, string spriteType)
    {
        switch (spriteType)
        {
            case "normal" :
                for (int i = 0; i < go.transform.childCount; ++i)
                {
                    if (go.transform.GetChild(i).gameObject.name == "Boyau")
                    {
                        go.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        go.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                break;
            case "normalLong":
                for (int i = 0; i < go.transform.childCount; ++i)
                {
                    if (go.transform.GetChild(i).gameObject.name == "BoyauLong")
                    {
                        go.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        go.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                break;
            case "stomach":
                for (int i = 0; i < go.transform.childCount; ++i)
                {
                    if (go.transform.GetChild(i).gameObject.name == "BoyauFinal")
                    {
                        go.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        go.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                break;
            case "stomachLong":
                for (int i = 0; i < go.transform.childCount; ++i)
                {
                    if (go.transform.GetChild(i).gameObject.name == "BoyauFinalLong")
                    {
                        go.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        go.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                break;
            case "selfLoop":
                for (int i = 0; i < go.transform.childCount; ++i)
                {
                    if (go.transform.GetChild(i).gameObject.name == "Boucle")
                    {
                        go.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        go.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                break;
            default:
                // instanciate normal
                for (int i = 0; i < go.transform.childCount; ++i)
                {
                    if (go.transform.GetChild(i).gameObject.name == "Boyau")
                    {
                        go.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        go.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                break;
        }
    }

}
