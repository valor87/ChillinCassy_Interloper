using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayDeathAnimation : MonoBehaviour
{
    public GameObject PlayerCamObject;
    public GameObject Interloper;
    EventCore eventCore;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Interloper.SetActive(false);
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventCore.death.AddListener(PlayJumpScare);
        animator = GetComponent<Animator>();
    }

    void PlayJumpScare(string death)
    {
        print(death);
        if (death == "Interloper")
        {
            Interloper.SetActive(true);
            PlayerCamObject.transform.eulerAngles = Vector3.zero;
            PlayerCamObject.GetComponent<PlayerCam>().enabled = false;
            animator.SetBool("PlayJumpScare", true);
            StartCoroutine(PlayDeathAnim());
        } 
        else
        {
            eventCore.loseGame.Invoke();
        }
    }
    IEnumerator PlayDeathAnim()
    {
        yield return new WaitForSeconds(1);
        eventCore.loseGame.Invoke();
    }
}
