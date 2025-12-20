using System.Collections;
using UnityEngine;

public class OpenPanelDoor : MonoBehaviour
{
    
    public Transform PanelDoor;
    Vector3 StartPos;
    public float PanelRotationDegrees;
    public bool CanStartRepair;
    private void Start()
    {
        StartPos = PanelDoor.eulerAngles;
    }
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(OpenDoorSlowly(PanelRotationDegrees));
    }
    private void OnTriggerExit(Collider other)
    {
        PanelDoor.eulerAngles = StartPos;
        PanelDoor.gameObject.SetActive(true);
        CanStartRepair = false;
    }
    IEnumerator OpenDoorSlowly(float AmountToTurnDegrees)
    {
        float turnAmount = AmountToTurnDegrees;
        float change = 0.1f;
        while (turnAmount > 0)
        {
            turnAmount -= change;
            PanelDoor.Rotate(0, 0, -change);
            yield return new WaitForSeconds(.0005f);
        }
        CanStartRepair = true;
        print("Door is open");
    }
}
