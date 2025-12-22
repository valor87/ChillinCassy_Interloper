using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class PowerBoxScript : MonoBehaviour
{
    //Set the camera infront of the power box
    public GameObject PanelDoor;
    public OpenPanelDoor Op;
    bool startRepair = false;
    [HideInInspector]
    public bool runrepair = false;
    [Header("Setting Camera Position")]
    public GameObject SceneCamera;
    [Header("For the buttons of the power box")]
    public GameObject NeedleParent;
    public GameObject ButtonParent;
    List<GameObject> AllButtons = new List<GameObject>();
    public Material PressedButtonMaterial;
    public Material UnPressedButtonMaterial;

    [Header("For the wires of the power box")]
    public GameObject PinkWire;
    public GameObject GreenWire;
    public Transform PinkWireEnd;
    public Transform GreenWireEnd;
    public float GreenRotateAmount;
    public float PinkRotateAmount;
    GameObject Wire;
    public GameObject EndPos;

    // Getting and setting the mouse position
    public LayerMask ButtonsLayer;
    Vector3 MousePos;
    Transform NeedleStartTransform;
    EventCore eventcore;
    [Space(10)]
    [Header("Manageing Sound")]
    public List<AudioClip> ButtonClicks;
    AudioSource AS;
    void Start()
    {
        AS = GetComponent<AudioSource>();
        NeedleStartTransform = NeedleParent.transform;
        eventcore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventcore.disableFog.AddListener(resetDial);
        AllButtons = SetListFromParent(ButtonParent);
        foreach (GameObject _Var in AllButtons)
        {
            _Var.GetComponent<MeshRenderer>().material = UnPressedButtonMaterial;
        }
    }
    void resetDial(string cause)
    {
        if (cause == "power")
        {
            PanelDoor.SetActive(true);
            NeedleParent.transform.localEulerAngles = new Vector3(0,0, -90);
            GreenWire.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
            PinkWire.GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
            GreenWire.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);
            PinkWire.GetComponent<LineRenderer>().SetPosition(1, Vector3.zero);

            GreenWire.GetComponent<BoxCollider>().enabled = true;
            PinkWire.GetComponent<BoxCollider>().enabled = true;
            foreach (GameObject _Var in AllButtons)
            {
                _Var.GetComponent<MeshRenderer>().material = UnPressedButtonMaterial;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        startRepair = Op.CanStartRepair;
        if (startRepair)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                runrepair = true;
            }
        }
        else
        {
            runrepair = false;
        }
        if (runrepair)
            PanelDoor.SetActive(false);
            RayCastInteraction();

    }
    void RayCastInteraction()
    {

        MousePos = Input.mousePosition;
        Ray MouseRayCast = SceneCamera.GetComponent<Camera>().ScreenPointToRay(MousePos);
        if (Physics.Raycast(MouseRayCast, out RaycastHit Hit, float.MaxValue, ButtonsLayer))
        {

            GameObject interactedObject = Hit.transform.gameObject;
            if (Input.GetMouseButtonDown(0))
            {
                if (interactedObject.tag == "IgnorePowerBox")
                {
                    return;
                }
                if (interactedObject.tag == "Wires")
                {
                    Wire = interactedObject;
                    WireDrag(interactedObject, interactedObject.transform.GetChild(0).gameObject, Hit.point);
                }
                else
                {
                    ButtonPressed(interactedObject, NeedleParent, 90);
                }
            }
            // the wire is attached to the players mouse
            if (Wire != null)
            {
                /*
                 *  this looks like cancer
                 *  its going to drive me into a tree
                 */
                if (interactedObject == GreenWireEnd.gameObject && Wire == GreenWire)
                {
                    // stops the player from touching the green box
                    GreenWire.GetComponent<BoxCollider>().enabled = false;
                    PinkWire.GetComponent<BoxCollider>().enabled = true;
                    // if the pink wire was connected, disconnect it
                    CheckLineCollison(PinkWire, NeedleParent, GreenRotateAmount);
                    // move the needle
                    NeedleParent.transform.Rotate(0, -PinkRotateAmount, 0);
                }
                else if (interactedObject == PinkWireEnd.gameObject && Wire == PinkWire)
                {
                    // stops the player from touching the pink wire
                    GreenWire.GetComponent<BoxCollider>().enabled = true;
                    PinkWire.GetComponent<BoxCollider>().enabled = false;
                    // if the green wire was connected, disconnect it
                    CheckLineCollison(GreenWire, NeedleParent, PinkRotateAmount);
                    // move the needle
                    NeedleParent.transform.Rotate(0, -GreenRotateAmount, 0);
                }
                ConnectWireToEndPos(interactedObject, NeedleParent, Hit.point);
            }
         
        }
    }
    void WireDrag(GameObject MoveableWire, GameObject Destination, Vector3 MousePos)
    {
        Vector3 StartPos = MoveableWire.transform.position;
        LineRenderer Wire = MoveableWire.GetComponent<LineRenderer>();
        Wire.SetPosition(0, StartPos);
        Wire.SetPosition(1, MousePos);
    }
    void ConnectWireToEndPos(GameObject WireDes, GameObject NeedlePiviot, Vector3 RaycastPosition)
    {
        float DialChangeInDegrees = 0;
        if (Wire == GreenWire)
        {
            DialChangeInDegrees = GreenRotateAmount;
        }
        else
        {
            DialChangeInDegrees = PinkRotateAmount;
        }
        Vector3 WireMousePos = RaycastPosition;
        Wire.GetComponent<LineRenderer>().SetPosition(1, WireMousePos);

        if (WireDes == Wire.transform.GetChild(0).gameObject)
        {
            Wire.GetComponent<LineRenderer>().SetPosition(1, WireDes.transform.position);
            

            Wire = null;
        }
    }
    void CheckLineCollison(GameObject TurnOffWire, GameObject NeedlePiviot, float RevertNeedleAmount)
    {
        NeedlePiviot.transform.Rotate(0, RevertNeedleAmount, 0);
        Vector3 LineCastFirstPos = TurnOffWire.GetComponent<LineRenderer>().GetPosition(0);
        TurnOffWire.GetComponent<LineRenderer>().SetPosition(1, LineCastFirstPos);
    }
    void ButtonPressed(GameObject Button, GameObject NeedlePivot, float RotateInDegrees)
    {
        AS.PlayOneShot(ButtonClicks[Random.RandomRange(0, ButtonClicks.Count)]);
        Material currentmaterial = Button.GetComponent<MeshRenderer>().material;
        if (currentmaterial.name == "Maroon (Instance)")
        {
            Button.GetComponent<MeshRenderer>().material = UnPressedButtonMaterial;
            NeedleParent.transform.Rotate(0, -RotateInDegrees, 0);
            return;
        }
        Button.GetComponent<MeshRenderer>().material = PressedButtonMaterial;
        NeedleParent.transform.Rotate(0, RotateInDegrees, 0);
    }

    static List<GameObject> SetListFromParent(GameObject Parent)
    {
        List<GameObject> ListofChildren = new List<GameObject>();
        int children = Parent.transform.childCount;
        for (int i = 0; i < children; i++)
        {
            ListofChildren.Add(Parent.transform.GetChild(i).gameObject);
        }
        return ListofChildren;
    }


}
