using UnityEngine;

public class Flashlight : MonoBehaviour
{
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
    }

    //check if the flashlight is looking at the interloper
    void checkForInterloper()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength, detectInterloper))
        {
            GameObject interloper = hit.transform.parent.gameObject;
            eventCore.detectedInterloper.Invoke(interloper);
        }
    }
}
