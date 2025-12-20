using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    //just connects the camera to the player externally
    public Transform cameraPosition;

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
