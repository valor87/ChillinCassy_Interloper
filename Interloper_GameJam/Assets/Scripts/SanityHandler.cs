using UnityEngine;

public class SanityHandler : MonoBehaviour
{
    public float sanity;

    bool fogEnabled;
    EventCore eventCore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        sanity = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (sanity <= 0 && !fogEnabled)
        {
            fogEnabled = true;
            eventCore.enableFog.Invoke();
        }
        else if (sanity > 0 && fogEnabled)
        {
            fogEnabled = false;
            eventCore.disableFog.Invoke();
        }
    }
}
