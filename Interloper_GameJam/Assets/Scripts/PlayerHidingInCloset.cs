using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHidingInCloset : MonoBehaviour
{
    [Header("For Closet MiniGame")]
    public GameObject QTEMiniGame;
    [Range(2, 10)]
    public int QTEGameRunTime;
    public bool RunGame;
    [Space(10)]
    [Header("Going in and out of the closet")]
    public Transform PlayerCurrentPos;
    public Transform RightClosetDoor;
    public Transform LeftClosetDoor;
    public Transform PlayerClosetPos;
    public float AjarDoorValue;
    public float PlayerExitAmount;
    Vector3 RightDoorRotation;
    Vector3 LeftDoorRotation;
    [Space(10)]
    [Header("Anti-Hiding Measures")]
    public GameObject hideWarning;
    public float maxHidingTime = 15;
    public float hidingDecayRate = 0.5f;
    Coroutine flashCoroutine;
    bool showingHideWarning;
    public float hidingTime;
    Vector3 OpenedDoor;
    bool CanOpenDoor = false;
    bool PlayerInsideCloset; // this name is sorta funny
    EventCore eventCore;
    private void OnTriggerEnter(Collider other)
    {
        
        StartCoroutine(OpenDoorSlowly(AjarDoorValue));
    }
    private void OnTriggerExit(Collider other)
    {
        RightClosetDoor.eulerAngles = RightDoorRotation;
        LeftClosetDoor.eulerAngles = LeftDoorRotation;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LeftDoorRotation = LeftClosetDoor.eulerAngles;
        RightDoorRotation = RightClosetDoor.eulerAngles;
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        OpenedDoor = new Vector3(0,95,0);
    }

    // Update is called once per frame
    void Update()
    {
        print(RightDoorRotation);
        PlayerInput(CanOpenDoor);
        if (RunGame)
        {
            StartCoroutine(StayingInClosetMiniGame(QTEGameRunTime));
        }

        processAntiHide();
    }
    // checks to see if the player wants to go
    // into the closet
    void PlayerInput(bool Caninteract)
    {
        if (!Caninteract)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PlayerInsideCloset)
            {
                StartCoroutine(MovePlayerOut(PlayerExitAmount));
            }
            LeftClosetDoor.eulerAngles = OpenedDoor;
            RightClosetDoor.eulerAngles = OpenedDoor;
            PlayerCurrentPos.position = PlayerClosetPos.position;
            PlayerInsideCloset = true;       
        }
    }
    /// <summary>
    /// Moves the player out 
    /// of the closet
    /// </summary>
    IEnumerator MovePlayerOut(float MoveAmount)
    {
        float ForwardMovement = MoveAmount;
        while (ForwardMovement > 0)
        {
            ForwardMovement -= 0.1f;
            PlayerCurrentPos.position += (Vector3.back/2);
            yield return new WaitForSeconds(.0005f);
        }
        PlayerInsideCloset = false;
    }
    /// <summary>
    /// Cute little door open
    /// to show its interactable
    /// </summary>
    IEnumerator OpenDoorSlowly(float AmountToTurnDegrees)
    {
        float turnAmount = AmountToTurnDegrees;
        float change = 0.1f;
        while(turnAmount > 0)
        {
           turnAmount -= change;
           RightClosetDoor.Rotate(0, -change, 0);
           yield return new WaitForSeconds(.0005f);
        }
        CanOpenDoor = true;
    }
    /// <summary>
    /// Starts and runs the mini game 
    /// until the player wins, loses, or the
    /// timer runs out
    /// </summary>
    IEnumerator StayingInClosetMiniGame(float RunGameForSeconds)
    {
        RunGame = false;
        QTEMiniGame.GetComponent<ClosetQTEMiniGame>().enabled = true;
        QTEMiniGame.GetComponent<ClosetQTEMiniGame>().SetUpTheGame();

        while (RunGameForSeconds > 0)
        {
            RunGameForSeconds -= Time.deltaTime;
            bool FaildGame = QTEMiniGame.GetComponent<ClosetQTEMiniGame>().FailedQTE;
            bool WinGame = QTEMiniGame.GetComponent <ClosetQTEMiniGame>().enabled;
            if (FaildGame) {
                print("You die");
                break;
            }
            if (!WinGame)
            {
                print("youWin");
            }
            yield return null;
        }
        QTEMiniGame.GetComponent<ClosetQTEMiniGame>().TurnOffAndOnAllObjects(QTEMiniGame, false);
    }

    //function for incrementing the hide time and deciding whether to show hide warning
    void processAntiHide()
    {
        if ((hidingTime / maxHidingTime) > (10f / 15f) && !showingHideWarning)
        {
            flashCoroutine = StartCoroutine(FlashHideWarning());
        }

        if (PlayerInsideCloset)
        {
            hidingTime += Time.deltaTime;
        }
        else
        {
            if (hidingTime > 0)
                hidingTime -= Time.deltaTime * hidingDecayRate;
            else
                hidingTime = 0;

            stopHideWarning(flashCoroutine);
        }

        if (hidingTime >= maxHidingTime)
        {
            PlayerInsideCloset = false;
            stopHideWarning(flashCoroutine);
            eventCore.death.Invoke("Dweller");
        }
    }

    //flash the hide warning when player hides for too long
    IEnumerator FlashHideWarning()
    {
        float[] percents = { 10f / 15f, 11f / 15f, 12f / 15f, 13f / 15f, 14f / 15f };
        float delay = 0.15f;
        showingHideWarning = true;
        while (true) 
        {
            float currentPercent = hidingTime / maxHidingTime;

            for (int i = 0; i < percents.Length; i++)
            {
                if (currentPercent < percents[i])
                {
                    //make it more opaque as it flashes more often
                    RawImage image = hideWarning.transform.GetChild(0).GetComponent<RawImage>();
                    Color newColor = image.color;
                    newColor.a = ((i + 1f) / 5f);
                    image.color = newColor;
                    hideWarning.transform.GetChild(0).GetComponent<RawImage>().color = image.color;

                    delay = 1f / (i + 1);
                    break;
                }
            }

            hideWarning.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            hideWarning.SetActive(false);
            yield return new WaitForSeconds(delay - 0.15f);

        }
    }

    void stopHideWarning(Coroutine coroutine)
    {
        if (coroutine != null) 
        {
            StopCoroutine(coroutine);
        }
        
        hideWarning.SetActive(false);
        showingHideWarning = false;
    }
    
}
