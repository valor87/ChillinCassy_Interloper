using UnityEditor;
using UnityEngine;

public class PlayDeathAnimation : MonoBehaviour
{
    public GameObject PlayerCamObject;
    public GameObject Interloper;
    EventCore eventCore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventCore.death.AddListener(PlayJumpScare);
    }

    
    void PlayJumpScare(string death)
    {
        Interloper.SetActive(true);
        PlayerCamObject.transform.eulerAngles = Vector3.zero;
        PlayerCamObject.GetComponent<PlayerCam>().enabled = false;

        GetComponent<Animator>().Play("Jumpscare");
    }
}
