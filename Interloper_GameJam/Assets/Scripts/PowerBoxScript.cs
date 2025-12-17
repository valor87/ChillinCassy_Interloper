using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using JetBrains.Annotations;
public class PowerBoxScript : MonoBehaviour
{
    //Set the camera infront of the power box
    [Header("Setting Camera Position")]
    public GameObject Camera;
    Vector3 CamPos;
    Vector3 CamRotation;

    [Header("Dials and Wire Parents")]
    public GameObject NeedleParent;
    public GameObject ButtonParent;
    GameObject ButtonLeft;
    GameObject ButtonRight;
    GameObject ButtonMiddle;
    List<GameObject> AllButtons = new List<GameObject>();
    public Material PressedButtonMaterial;
    public Material UnPressedButtonMaterial;

    // Getting and setting the mouse position
    public LayerMask ButtonsLayer;
    Vector3 MousePos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AllButtons = SetListFromParent(ButtonParent);
        CamPos = transform.position - new Vector3(0,0,2f);
        Camera.transform.position = CamPos;
        foreach (GameObject _Var in AllButtons)
        {
            GameObject colorchange = _Var.transform.GetChild(0).gameObject;
            colorchange.GetComponent<MeshRenderer>().material = UnPressedButtonMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MousePos = Input.mousePosition;
        Ray MouseRayCast = Camera.GetComponent<Camera>().ScreenPointToRay(MousePos);
        if (Physics.Raycast(MouseRayCast, out RaycastHit Hit, float.MaxValue, ButtonsLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                ButtonPressed(Hit.transform.gameObject, NeedleParent, 90);
            }
        }
    }

    void ButtonPressed(GameObject Button, GameObject NeedlePivot, float RotateInDegrees)
    {
        Material currentmaterial = Button.GetComponent<MeshRenderer>().material;
        print(currentmaterial.name);
        if (currentmaterial.name == "Maroon (Instance)")
        {
            Button.GetComponent<MeshRenderer>().material = UnPressedButtonMaterial;
            NeedleParent.transform.Rotate(0, 0, -RotateInDegrees);
            return;
        }
        Button.GetComponent<MeshRenderer>().material = PressedButtonMaterial;
        NeedleParent.transform.Rotate(0,0,RotateInDegrees);
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
