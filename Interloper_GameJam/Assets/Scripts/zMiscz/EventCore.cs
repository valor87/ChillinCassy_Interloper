using UnityEngine;
using UnityEngine.Events;

public class EventCore : MonoBehaviour
{
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    //event for opening the gate in the intro
    [HideInInspector]
    public UnityEvent EV_openGate;
    public UnityEvent EV_cameraAnimationEnd;
    //event for player death.
    //string is for the cause of death, either by interloper, crying face or tickler (anti-hide measures)
    [HideInInspector]
    public UnityEvent<string> death;

    //event for detecting an interloper with a flashlight.
    //gameObject is for the interloper in question since there might be multiple
    [HideInInspector]
    public UnityEvent<GameObject> detectedInterloper;

    //event for an interloper moving a bookshelf to unblock entrance
    //gameObject is for the bookshelf
    [HideInInspector]
    public UnityEvent<GameObject> unblockBookshelf;

    //event for an interloper moving a bookshelf to block entrance
    //gameObject is for the bookshelf
    [HideInInspector]
    public UnityEvent<GameObject> blockBookshelf;

    //event for updating sanity, either increasing or decreasing
    //float is the amount of value in sanity
    [HideInInspector]
    public UnityEvent<float> updateSanity;

    //event for making fog appear
    //string is for the condition (sanity, power)
    [HideInInspector]
    public UnityEvent<string> enableFog;

    //event for making fog disappear
    //string is for the condition (sanity, power)
    [HideInInspector]
    public UnityEvent<string> disableFog;

    //event for winning game
    [HideInInspector]
    public UnityEvent loseGame;

    [HideInInspector]
    public UnityEvent winGame;
}
