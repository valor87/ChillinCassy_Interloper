using UnityEngine;

public class SanityHandler : MonoBehaviour
{
    public float sanity;
    public float sanityDecreaseRate = 0.5f;

    bool fogEnabled;
    EventCore eventCore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventCore.updateSanity.AddListener(changeSanity);
        sanity = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (sanity <= 0 && !fogEnabled)
        {
            fogEnabled = true;
            eventCore.enableFog.Invoke("sanity");
        }
        else if (sanity > 0 && fogEnabled)
        {
            fogEnabled = false;
            eventCore.disableFog.Invoke("sanity");
        }
    }

    private void LateUpdate()
    {
        changeSanity(Time.deltaTime * -sanityDecreaseRate);
    }

    //change the sanity
    void changeSanity(float value)
    {
        sanity += value;

        if (sanity > 100)
            sanity = 100;
        else if (sanity < 0)
            sanity = 0;
    }
}
