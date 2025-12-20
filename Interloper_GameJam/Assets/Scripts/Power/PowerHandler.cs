using UnityEngine;

public class PowerHandler : MonoBehaviour
{
    public bool powerEnabled;
    
    [Header("Values")]
    //percent chance of power breaking
    public float powerBreakFreq;
    //amount of time needed for a power break to happen. in seconds
    public float powerBreakTimeWindow;

    float timer;
    EventCore eventCore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerEnabled = true;
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > powerBreakTimeWindow)
        {
            PowerBreakCheck();
            timer = 0;
        }
    }

    //check if a interloper should be spawned by chance
    void PowerBreakCheck()
    {
        if (Random.Range(1, 101) <= powerBreakFreq)
        {
            powerEnabled = false;
            eventCore.enableFog.Invoke("power");
        }
    }
}
