using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Transform cameraTransform;
    public float rayLength;
    public LayerMask detectInterloper;

    EventCore eventCore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
    }

    // Update is called once per frame
    void Update()
    {
        checkForInterloper();
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward, Color.yellow);
    }

    //check if the flashlight is looking at the interloper
    void checkForInterloper()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, rayLength, detectInterloper))
        {
            print("scared away interloper");
            print(hit.collider.gameObject.name);
            //GameObject interloper = hit.transform.parent.gameObject;
            GameObject interloper = hit.collider.gameObject;
            eventCore.detectedInterloper.Invoke(interloper);
        }
    }
}
