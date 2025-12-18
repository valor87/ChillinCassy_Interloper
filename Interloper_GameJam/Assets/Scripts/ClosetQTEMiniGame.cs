using UnityEngine;

public class ClosetQTEMiniGame : MonoBehaviour
{
    public RectTransform PlayerObject;
    public RectTransform FirstCheck;
    public RectTransform SecondCheck;
    public float Speed;
    public bool FailedQTE;
    bool PlayerAttemptedhit;
    bool ConfirmedHit;
    Vector3 ChangeplayerPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeplayerPos = Vector3.right * 5;
        FailedQTE = false;
        Vector3 RandomX = FirstCheck.position;
        RandomX.x = Random.RandomRange(100, 1800);
        FirstCheck.position = RandomX;
        RandomX = SecondCheck.position;
        RandomX.x = Random.RandomRange(100, 1800);
        SecondCheck.position = RandomX;
    }

    // Update is called once per frame
    void Update()
    {
        ConfirmedHit = false;
        PlayerInputCheck();
        PlayerIconMovement();
        bool FirstCheckActive = FirstCheck.gameObject.activeInHierarchy;
        bool SecondCheckActive = SecondCheck.gameObject.activeInHierarchy;

        if (FirstCheckActive)
        {
            if (CheckCollision(FirstCheck) && PlayerAttemptedhit)
            {
                ConfirmedHit = true;
                print("hit");
                FirstCheck.gameObject.SetActive(false);
            }
        }
        if (SecondCheckActive)
        {
            if (CheckCollision(SecondCheck) && PlayerAttemptedhit)
            {
                ConfirmedHit = true;
                print("hit");
                SecondCheck.gameObject.SetActive(false);
            }
        }
        
        if (!FirstCheckActive && !SecondCheckActive)
        {
            this.enabled = false;
        }

        if (!ConfirmedHit && PlayerAttemptedhit)
        {
            FailedQTE = true;
            print("Dummy you missed");
        }

    }
    // sets the game to start
    public void SetUpTheGame()
    {
        FailedQTE = false;
        TurnOffAndOnAllObjects(this.gameObject, true);
        ChangeplayerPos = Vector3.right * 5;

        Vector3 RandomX = FirstCheck.position;
        RandomX.x = Random.RandomRange(100, 1800);
        FirstCheck.position = RandomX;
        RandomX = SecondCheck.position;
        RandomX.x = Random.RandomRange(100, 1800);
        SecondCheck.position = RandomX;
    }
    // switches the visablity of the children
    public void TurnOffAndOnAllObjects(GameObject Parent, bool Status)
    {
        for (int i = 0; i < Parent.transform.childCount; i++)
        {
            Parent.transform.GetChild(i).gameObject.SetActive(Status);
        }
    }
    // get input from the player
    void PlayerInputCheck()
    {

        if (Input.GetMouseButtonDown(0))
        {
            PlayerAttemptedhit = true;
        }
        else
        {
            PlayerAttemptedhit = false;
        }
        
    }
    /// <summary>
    ///  moveing the player icon based
    ///  on a speed and changes direction when too
    ///  close to one side
    /// </summary>
    void PlayerIconMovement()
    {
        PlayerObject.position -= ChangeplayerPos * Speed * Time.deltaTime;
        if (PlayerObject.position.x <= 100)
        {
            ChangeplayerPos.x = ChangeplayerPos.x * -1;
        }
        if (PlayerObject.position.x >= 1800)
        {
            ChangeplayerPos.x = ChangeplayerPos.x * -1;
        }
    }
    /// <summary>
    /// checks if the player object is
    /// touching one of the check icons
    /// </summary>
    bool CheckCollision(RectTransform Collison)
    {
        if (PlayerObject.position.x >= Collison.position.x - 50 && PlayerObject.position.x <= Collison.position.x + 50)
        {
            return true;
        }
        return false;
    }
}
