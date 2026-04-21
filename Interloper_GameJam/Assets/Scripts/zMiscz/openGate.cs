using System.Collections;
using UnityEngine;

public class openGate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float targetRotation;
    public float turnAmount = 5;
    public CameraAnimation CameraAni;
    EventCore EventCore;
    void Start()
    {
        EventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        EventCore.EV_openGate.AddListener(startAnimation);
    }
    [ContextMenu("Open Gate")]
    void startAnimation()
    {
        this.CameraAni.startCameraMovement();
        StartCoroutine(openGateSlow());
    }
    IEnumerator openGateSlow()
    {
        float currentRotation = transform.eulerAngles.y;
        while (currentRotation >= targetRotation)
        {
            currentRotation -= turnAmount * Time.deltaTime;
            transform.eulerAngles = new Vector3(0, currentRotation, 0);
            yield return null;
        }
    }
}
