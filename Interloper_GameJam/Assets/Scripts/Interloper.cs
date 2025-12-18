using UnityEngine;
using UnityEngine.AI;

public class Interloper : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent ai;
    public Transform player;
    public Transform[] returnPoints;

    [Header("General")]
    public float moveSpeed = 5;
    public float returnDistance = 5;

    EventCore eventCore;
    bool returnToPoint;
    Transform chosenReturnPoint;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventCore.death.AddListener(debugReset);
        eventCore.detectedInterloper.AddListener(determineDetection);
    }

    // Update is called once per frame
    void Update()
    {
        doMovement();
    }

    void debugReset(string causeOfDeath)
    {
        print("reset");
        if (causeOfDeath == "Interloper")
        {
            ai.Warp(new Vector3(100, 0, 0));
        }
        
    }

    //movement for the interloper. either moves towards player or returns
    void doMovement()
    {
        
        //move towards player
        if (!returnToPoint)
        {
            ai.speed = moveSpeed;
            ai.destination = player.position;
        }
        //return to a point
        else
        {
            ai.speed = moveSpeed * 4;
            ai.destination = chosenReturnPoint.position;

            Vector3 directionVector = ai.gameObject.transform.position - ai.destination;
            if (directionVector.magnitude < returnDistance)
            {
                returnToPoint = false;
            }
        }

    }

    //check if the interloper has been detected
    void determineDetection(GameObject interloper)
    {
        if (interloper == gameObject)
        {
            returnToPoint = true;
            chosenReturnPoint = returnPoints[Random.Range(0, returnPoints.Length)];
        }
    }
}
