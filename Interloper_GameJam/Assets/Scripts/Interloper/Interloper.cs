using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Interloper : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent ai;
    public Transform player;
    public Transform returnPoint;
    public Transform houseEscapePoint;
    public GameObject interloperSpot;
    public List<Bookshelf> bookshelfList;
    public Animator animator;

    [Header("General")]
    public float moveSpeed = 5;
    public float returnDistance = 5;
    public float interloperAutoKill = 20;

    EventCore eventCore;
    [Header("Display Variables")]
    public bool returnToPoint;
    public bool allBookshelvesCovered;
    bool playerInCloset;
    float autoKillTimer;
    bool escapingThroughRoof;

    Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventCore.death.AddListener(debugReset);
        eventCore.detectedInterloper.AddListener(determineDetection);
        eventCore.blockBookshelf.AddListener(CheckBookshelves);
        eventCore.unblockBookshelf.AddListener(CheckBookshelves);

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
        //if (causeOfDeath == "Interloper")
        //    ai.Warp(new Vector3(-31, -12, 21));
        Destroy(this.gameObject);
    }
  
    //movement for the interloper. either moves towards player or returns
    void doMovement()
    {
        autoKillTimer = Time.deltaTime;

        if (escapingThroughRoof)
        {
            //currently just makes it fly upwards
            animator.enabled = false;
            Vector3 newPos = transform.position;
            newPos.y += 1 * Time.deltaTime;
            transform.position = newPos;

            if (transform.position.y >= 5)
            {
                Destroy(gameObject);
            }

            return;
        }
        
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

            //make interloper leave the building through bookshelf opening if possible
            if (!allBookshelvesCovered)
            {
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
            //if all bookshelves are closed, then it will jump through the roof
            else
            {
                ai.destination = houseEscapePoint.position;

                Vector3 directionVector = ai.gameObject.transform.position - ai.destination;
                if (directionVector.magnitude < returnDistance / 2)
                {
                    escapingThroughRoof = true;
                    ai.enabled = false;
                    rb.useGravity = false;
                    //swap interloper's animation to climbing the wall here
                    //animator.play("crawling")
                }
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
        CheckBookshelves(null);
        //chosenReturnPoint = returnPoints[Random.Range(0, returnPoints.Length)];
    }

    //checks if there's an open bookshelf to escape through
    void CheckBookshelves(GameObject _bookshelf)
    {
        print("checking the bookshelves");
        foreach (var bookshelf in bookshelfList)
        {
            if (!bookshelf.activelyBlocking)
            {
                print("there is one free bookshelf");
                allBookshelvesCovered = false;
                returnPoint = bookshelf.transform.GetChild(0);
                return;
            }
        }

        print("all bookshelves are covered");
        allBookshelvesCovered = true;
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

    private void OnTriggerEnter(Collider other)
    {
        //print("detected something");
        //print(collision.gameObject);
        if (!other.gameObject.CompareTag("Bookshelf"))
            return;

        //print("detected bookshelf");

        Bookshelf collidedBookshelf = other.gameObject.GetComponent<Bookshelf>();

        if (!collidedBookshelf.activelyBlocking)
            eventCore.blockBookshelf.Invoke(other.gameObject);
        else
            eventCore.unblockBookshelf.Invoke(other.gameObject);

        returnBackToPoint();
    }
}
