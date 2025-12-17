using UnityEngine;
using UnityEngine.AI;

public class Interloper : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent ai;
    public Transform player;

    [Header("General")]
    public float moveSpeed = 5;

    EventCore eventCore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventCore.death.AddListener(debugReset);
    }

    // Update is called once per frame
    void Update()
    {
        //move towards player
        ai.speed = moveSpeed;
        ai.destination = player.position;
    }

    void debugReset(string causeOfDeath)
    {
        print("reset");
        if (causeOfDeath == "Interloper")
        {
            ai.Warp(new Vector3(100, 0, 0));
        }
        
    }
}
