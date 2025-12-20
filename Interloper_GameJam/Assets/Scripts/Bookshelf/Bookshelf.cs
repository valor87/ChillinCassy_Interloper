using System.Collections;
using UnityEngine;

public class Bookshelf : MonoBehaviour
{
    public bool activelyBlocking;
    public Vector3 unblockDestination;
    public float moveSpeed;
    EventCore eventCore;
    Vector3 originalPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventCore.unblockBookshelf.AddListener(StopBlocking);
        eventCore.blockBookshelf.AddListener(StartBlocking);

        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StopBlocking(GameObject bookshelf)
    {
        //print("test stopBlocking");
        if (bookshelf != gameObject)
            return;

        //print("stopBlocking successful");
        activelyBlocking = false;
        StopAllCoroutines();
        StartCoroutine(Movement(unblockDestination));
    }

    void StartBlocking(GameObject bookshelf)
    {
        //print("test startBlocking");
        if (bookshelf != gameObject)
            return;

        //print("startBlocking successful");
        activelyBlocking = true;
        StopAllCoroutines();
        StartCoroutine(Movement(originalPosition));
    }

    IEnumerator Movement(Vector3 destination)
    {
        //print("started movement");
        while (true)
        {
            
            //Vector3.MoveTowards(transform.position, destination, moveSpeed);
            Vector3 directionVector = destination - transform.position;
            transform.position += directionVector.normalized * moveSpeed * 0.1f;
            //print("moving: " + directionVector.magnitude);
            if (directionVector.magnitude < 1)
            {
                transform.position = destination;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        
    } 
}
