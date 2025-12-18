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
    }

    // Update is called once per frame
    void Update()
    {
        if (FirstCheck != null)
        {
            CheckCollision(FirstCheck);

        }
        if (SecondCheck != null)
        {
            CheckCollision(SecondCheck);
        }
        if (SecondCheck == null && FirstCheck == null)
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
                Destroy(Collison.gameObject);
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
