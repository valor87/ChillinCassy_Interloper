using UnityEngine;
using UnityEngine.Events;

public class EventCore : MonoBehaviour
{
    //event for player death.
    //string is for the cause of death, either by interloper, crying face or tickler (anti-hide measures)
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

    //event for updating sanity, either increasing or decreasing
    //float is the amount of value in sanity
    public UnityEvent<float> updateSanity;

    //event for making fog appear
    //string is for the condition (sanity, power)
    public UnityEvent<string> enableFog;

    //event for making fog disappear
    //string is for the condition (sanity, power)
    public UnityEvent<string> disableFog;

    //event for winning game
    public UnityEvent winGame;
}
