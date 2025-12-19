using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Transform cameraTransform;
    public float rayLength;
    public LayerMask flashMonster;

    EventCore eventCore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
    }

    // Update is called once per frame
    void Update()
    {
        checkForMonster();
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward, Color.yellow);
    }

    //check if the flashlight is looking at the interloper or static monster
    void checkForMonster()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, rayLength, flashMonster))
        {
            print("scared away interloper");
            print(hit.collider.gameObject.name);
            if (hit.collider.gameObject.GetComponentInParent<Interloper>() != null)
            {
                //GameObject interloper = hit.transform.parent.gameObject;
                GameObject interloper = hit.collider.gameObject;
                eventCore.detectedInterloper.Invoke(interloper);
            }
            else if (hit.collider.gameObject.GetComponentInParent<StaticMonster>() != null)
            {
                GameObject staticMonster = hit.transform.parent.gameObject;
                Destroy(staticMonster);
            }

        }
    }
}
