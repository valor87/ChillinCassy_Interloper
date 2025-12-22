using UnityEngine;
using UnityEngine.AI;

public class Interloper : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent ai;
    public Transform player;
    public Transform returnPoint;
    public GameObject interloperSpot;

    [Header("General")]
    public float moveSpeed = 5;
    public float returnDistance = 5;
    public float interloperAutoKill = 20;

    EventCore eventCore;
    public bool returnToPoint;
    bool playerInCloset;
    float autoKillTimer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventCore.death.AddListener(debugReset);
        eventCore.detectedInterloper.AddListener(determineDetection);
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        doMovement();
        //band-aid fix for interlopers getting attracted to closet
        playerInCloset = interloperSpot.transform.parent.GetComponent<PlayerHidingInCloset>().PlayerInsideCloset;
    }

    void debugReset(string causeOfDeath)
    {
        //print("reset");
        if (causeOfDeath == "Interloper")
            ai.Warp(new Vector3(-31, -12, 21));
        
    }
    //
    void PlayJumpScare()
    {

    }
    //movement for the interloper. either moves towards player or returns
    void doMovement()
    {
        autoKillTimer = Time.deltaTime;
        
        if (playerInCloset)
        {
            ai.speed = moveSpeed * 0.5f;
            ai.destination = interloperSpot.transform.position;
        }
        //move towards player
        else if (!returnToPoint)
        {
            ai.speed = moveSpeed;
            ai.destination = player.position;
        }

        //return to a point
        if (returnToPoint)
        {
            ai.speed = moveSpeed * 4;
            ai.destination = returnPoint.position;

            Vector3 directionVector = ai.gameObject.transform.position - ai.destination;
            if (directionVector.magnitude < returnDistance || autoKillTimer > interloperAutoKill)
            {
                //print("return to the damn point");
                //ai.velocity = Vector3.zero;
                //returnToPoint = false;
                Destroy(gameObject);
            }
        }
    }

    //check if the interloper has been detected
    void determineDetection(GameObject interloper)
    {
        //print("determining detection");
        //print(interloper.transform.parent.name);
        //print(gameObject.name);

        //check if the interloper received is this one since this gets sent to every interloper
        //if (interloper == gameObject)
        if (interloper.transform.parent.gameObject == gameObject && !playerInCloset)
        {
            returnBackToPoint();
        } 
    }

    public void returnBackToPoint()
    {
        //print("returning back to point");
        returnToPoint = true;
        //chosenReturnPoint = returnPoints[Random.Range(0, returnPoints.Length)];
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print("detected something");
        //print(collision.gameObject);
        if (!collision.gameObject.CompareTag("Bookshelf"))
            return;

        //print("detected bookshelf");

        Bookshelf collidedBookshelf = collision.gameObject.GetComponent<Bookshelf>();

        if (!collidedBookshelf.activelyBlocking)
            eventCore.blockBookshelf.Invoke(collision.gameObject);
        else
            eventCore.unblockBookshelf.Invoke(collision.gameObject);

        returnBackToPoint();
    }
}
