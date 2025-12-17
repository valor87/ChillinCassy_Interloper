using UnityEngine;
using UnityEngine.Events;

public class EventCore : MonoBehaviour
{
    //event for player death.
    //string is for the cause of death, either by interloper, static monster or bed dweller
    public UnityEvent<string> death;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
