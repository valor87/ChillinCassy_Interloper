using UnityEngine;

public class ClosetQTEMiniGame : MonoBehaviour
{
    public RectTransform PlayerObject;
    public RectTransform FirstCheck;
    public float Speed;
    Vector3 ChangeplayerPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeplayerPos = (Vector3.right/2);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerObject.position.x >= FirstCheck.position.x - 100 && PlayerObject.position.x <= FirstCheck.position.x + 100)
        {
            print("Hit the button");
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
