using System.Collections;
using UnityEngine;

public class CameraAnimationManager : MonoBehaviour
{
    [SerializeField]
    CameraAnimation[] animations;

    [ContextMenu("Play animations")]
    void playAllAnimations()
    {
        StartCoroutine(manageAnimation());
    }

    IEnumerator manageAnimation()
    {
        foreach (var animation in animations)
        {
            yield return animation.camMovement();
        }
    }
}
