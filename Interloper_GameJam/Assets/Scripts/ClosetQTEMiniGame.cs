using UnityEngine;

public class ClosetQTEMiniGame : MonoBehaviour
{
    public RectTransform PlayerObject;
    public RectTransform FirstCheck;
    public RectTransform SecondCheck;
    public float Speed;
    Vector3 ChangeplayerPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeplayerPos = (Vector3.right / 2);

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
        bool FirstCheckActive = FirstCheck.gameObject.activeInHierarchy;
        bool SecondCheckActive = SecondCheck.gameObject.activeInHierarchy;
        if (FirstCheck != null)
        {
            CheckCollision(FirstCheck);

        }
        if (SecondCheck != null)
        {
            CheckCollision(SecondCheck);
        }
        if (!FirstCheckActive && !SecondCheckActive)
        {
            this.gameObject.SetActive(false);
        }
    }
    void CheckCollision(RectTransform Collison)
    {
        if (PlayerObject.position.x >= Collison.position.x - 100 && PlayerObject.position.x <= Collison.position.x + 100)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Collison.gameObject.SetActive(false);
            }
        }
        PlayerObject.position -= ChangeplayerPos * Speed;
        if (PlayerObject.position.x <= 100)
        {
            ChangeplayerPos.x = ChangeplayerPos.x * -1;
        }
        if (PlayerObject.position.x >= 1800)
        {
            ChangeplayerPos.x = ChangeplayerPos.x * -1;
        }
    }
}
