using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public Transform orientation;
    public float groundDrag;

    [Header("References")]
    public GameObject flashlight;

    [Header("Detection")]
    public float detectionRayLength;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;
    EventCore eventCore;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventCore.death.AddListener(debugRespawn);
    }

    void Update()
    {
        GetInput();
        SpeedControl();
        rb.linearDamping = groundDrag; 
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //if right mouse click is pressed, turn on flashlight
        if (Input.GetMouseButtonDown(1))
        {
            flashlight.SetActive(!flashlight.activeSelf);
        }

        if (Input.GetMouseButtonDown(0))
        {
            checkForBookshelf();
        }
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    //limits the velocity of the player's rigidbody
    void SpeedControl()
    {
        Vector3 currentVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (currentVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = currentVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //checks in the parent (since the object that holds the collider is a child) for the interloper
        if (collision.gameObject.GetComponentInParent<Interloper>())
        {
            eventCore.death.Invoke("Interloper");
        }
        else if (collision.gameObject.GetComponentInParent<CryingFace>())
        {
            eventCore.updateSanity.Invoke(-10);
            Destroy(collision.transform.parent.gameObject);
        }
    }

    void debugRespawn(string causeOfDeath)
    {
        print("you died!!!! cause: " + causeOfDeath);
        transform.position = new Vector3(8, 15, 0);
    }

    void checkForBookshelf()
    {
        print("checking for bookshelf");
        if (Physics.Raycast(transform.position, orientation.forward, out RaycastHit hit, detectionRayLength))
        {
            print("hitting: " + hit.collider.name);
            if (!hit.collider.gameObject.CompareTag("Bookshelf"))
                return;

            print("bookshelf has been detected");
            GameObject bookshelf = hit.collider.gameObject;

            if (!bookshelf.GetComponent<Bookshelf>().activelyBlocking)
                eventCore.blockBookshelf.Invoke(bookshelf);
        }



    }
}
