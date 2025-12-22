using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public Transform orientation;
    public float groundDrag;

    [Header("References")]
    public GameObject flashlight;
    public FogHandler fogHandler;
    public GameObject DeathScreen;
    [Header("Detection")]
    public float detectionRayLength;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;
    EventCore eventCore;
    float originalSpeed;
    [Header("SoundEffects")]
    public SoundManager soundManager;
    public AudioClip Flashlightclick;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventCore.loseGame.AddListener(debugRespawn);
        originalSpeed = moveSpeed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            eventCore.loseGame.Invoke();
        }
        GetInput();
        SpeedControl();
        ChangePlayerSpeed();
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
           
            bool flashlightEnabled = flashlight.GetComponent<Flashlight>().flashlightEnabled;
            
            flashlight.GetComponent<Flashlight>().flashlightEnabled = !flashlightEnabled;
            if (!flashlightEnabled)
            {
                soundManager.PLayOneShot(Flashlightclick);
            }
            print(!flashlightEnabled);
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

    //if no sanity and no power, player moves more slower
    void ChangePlayerSpeed()
    {
        if (!fogHandler.CheckBothConditions())
        {
            moveSpeed = originalSpeed;
        }
        else
        {
            moveSpeed = originalSpeed / 2;
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

    void debugRespawn()
    {
        print("you dead");
        DeathScreen.SetActive(true);
        DeathScreen.GetComponent<Animator>().SetBool("DeathScene", true);
        transform.position = new Vector3(8, 15, 0);
        StartCoroutine(ItFoundYouAnim());
    }
    IEnumerator ItFoundYouAnim()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
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
