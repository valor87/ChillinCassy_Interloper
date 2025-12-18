using System.Collections;
using UnityEngine;

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
    Vector3 OpenedDoor;
    bool CanOpenDoor = false;
    bool PlayerInsideCloset; // this name is sorta funny
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(OpenDoorSlowly(AjarDoorValue));
    }
    private void OnTriggerExit(Collider other)
    {
        RightClosetDoor.eulerAngles = new Vector3(0,-180,0);
        LeftClosetDoor.eulerAngles = Vector3.zero;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OpenedDoor = new Vector3(0,95,0);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput(CanOpenDoor);
        if (RunGame)
        {
            StartCoroutine(StayingInClosetMiniGame(QTEGameRunTime));
        }
    
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
    /// Cute little door oprn
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
}
