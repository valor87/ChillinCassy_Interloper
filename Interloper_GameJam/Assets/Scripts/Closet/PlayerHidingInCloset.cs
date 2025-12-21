using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerHidingInCloset : MonoBehaviour
{
    [Header("For Closet MiniGame")]
    public GameObject QTEMiniGame;
    public GameObject InterloperWaitingSpot;
    [UnityEngine.Range(2, 10)]
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
    Coroutine OpeningDoor;
    [Space(10)]
    [Header("Anti-Hiding Measures")]
    public float sanityRecoverRate = 9;
    public GameObject hideWarning;
    public float maxHidingTime = 15;
    public float hidingDecayRate = 0.5f;
    Coroutine flashCoroutine;
    bool showingHideWarning;
    public float hidingTime;
    Vector3 OpenedDoor;
    bool CanOpenDoor = false;
    public bool PlayerInsideCloset; // this name is sorta funny
    EventCore eventCore;
    private void OnTriggerEnter(Collider other)
    {
        OpeningDoor = StartCoroutine(OpenDoorSlowly(AjarDoorValue));
    }
    private void OnTriggerExit(Collider other)
    {
        RightClosetDoor.eulerAngles = RightDoorRotation;
        LeftClosetDoor.eulerAngles = LeftDoorRotation;
        StopCoroutine(OpeningDoor);
        CanOpenDoor = false;
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
        //print(RightDoorRotation);
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
            InterloperWaitingSpot.SetActive(true);
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
            PlayerCurrentPos.position -= (Vector3.right/2);
            yield return new WaitForSeconds(.0005f);
        }
        InterloperWaitingSpot.SetActive(false);
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

        while (true)
        {
            RunGameForSeconds -= Time.deltaTime;
            bool FaildGame = QTEMiniGame.GetComponent<ClosetQTEMiniGame>().FailedQTE;
            bool WinGame = !QTEMiniGame.GetComponent<ClosetQTEMiniGame>().enabled;
            if (FaildGame || RunGameForSeconds <= 0) {
                print("You die");
                eventCore.death.Invoke("Tickler");
                break;
            }
            if (WinGame)
            {
                print("youWin");
                break;
            }
            yield return null;
        }

        List<GameObject> interlopers = InterloperWaitingSpot.GetComponent<MinigameStarter>().interlopers;
        for (int i = 0; i < interlopers.Count; i++)
        {
            GameObject interloper = interlopers[i];
            interloper.GetComponent<Interloper>().returnBackToPoint();
        }
        interlopers.Clear();
        InterloperWaitingSpot.GetComponent<MinigameStarter>().minigameAlreadyPlayed = false;
        //InterloperWaitingSpot.gameObject.SetActive(false);
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
            CanOpenDoor = true;
            eventCore.updateSanity.Invoke(sanityRecoverRate * Time.deltaTime);
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
            eventCore.death.Invoke("Tickler");
        }
    }

    //flash the hide warning when player hides for too long
    IEnumerator FlashHideWarning()
    {
        showingHideWarning = true;

        while (true) 
        {
            hideWarning.SetActive(true);
            hideWarning.GetComponent<Animator>().Play("TickleAnim");
            yield return null;
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
