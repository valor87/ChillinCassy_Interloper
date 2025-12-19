using UnityEngine;
using UnityEngine.Events;

public class EventCore : MonoBehaviour
{
    //event for player death.
    //string is for the cause of death, either by interloper, static monster or dweller (anti-hide measures)
    public UnityEvent<string> death;

    //event for detecting an interloper with a flashlight.
    //string is for the interloper in question since there might be multiple
    public UnityEvent<GameObject> detectedInterloper;

    //event for an interloper moving a bookshelf to unblock entrance
    //string is for the bookshelf
    public UnityEvent<GameObject> unblockBookshelf;

    //event for an interloper moving a bookshelf to block entrance
    //string is for the bookshelf
    public UnityEvent<GameObject> blockBookshelf;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
