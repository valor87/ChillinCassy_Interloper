using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public bool flashlightEnabled;
    [Header("References")]
    public Transform cameraTransform;
    public GameObject flashlightObj;
    public LayerMask flashMonster;
    [Header("Values")]
    public float rayLength;
    public float batteryLengthSeconds; //length in seconds
    public float batteryRechargeRate;

    bool flashlightExhaustion;
    float batteryAmount;
    EventCore eventCore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        batteryAmount = batteryLengthSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (flashlightEnabled && !flashlightExhaustion)
        {
            CheckForMonster();
            ChangeBattery(-1);
            flashlightObj.SetActive(true);
            GetComponent<Collider>().enabled = true;
        }
        else
        {
            ChangeBattery(batteryRechargeRate);
            flashlightObj.SetActive(false);
            GetComponent<Collider>().enabled = false;
        }

        Debug.DrawRay(cameraTransform.position, cameraTransform.forward, Color.yellow);
    }

    //check if the flashlight is looking at the interloper or static monster
    void CheckForMonster()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, rayLength, flashMonster))
        {
            //print("scared away interloper");
            //print(hit.collider.gameObject.name);
            if (hit.collider.gameObject.GetComponentInParent<Interloper>() != null)
            {
                //GameObject interloper = hit.transform.parent.gameObject;
                GameObject interloper = hit.collider.gameObject;
                eventCore.detectedInterloper.Invoke(interloper);
            }
            else if (hit.collider.gameObject.GetComponentInParent<CryingFace>() != null)
            {
                GameObject cryingFace = hit.transform.parent.gameObject;
                Destroy(cryingFace);
            }

        }
    }

    void ChangeBattery(float rate)
    {
        batteryAmount += Time.deltaTime * rate;

        if (batteryAmount > batteryLengthSeconds)
        {
            batteryAmount = batteryLengthSeconds;
            flashlightExhaustion = false;
        }
        else if (batteryAmount < 0)
        {
            flashlightEnabled = false;
            flashlightExhaustion = true;
            batteryAmount = 0;
        }
    }
}
