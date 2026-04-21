using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
public class PowerBoxEndCollsion : MonoBehaviour
{
    public Transform PowerBox;
    public EventCore EventCore;
    public PowerBoxScript PowerBoxScript;
    public List<Vector3> RandomPos = new List<Vector3>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        EventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        EventCore.enableFog.AddListener(SetPos);
    }
    private void OnTriggerEnter(Collider other)
    {
        print("you win");
        SwitchCollison(false);
        EventCore.disableFog.Invoke("power");
        PowerBoxScript.runrepair = false;
    }
    void SetPos(string Cause)
    {
        print(Cause);
        if (Cause == "power")
        {
            SwitchCollison(true);
            int num = Random.Range(0, RandomPos.Count);
            transform.localPosition = RandomPos[num];
        }
    }
    void SwitchCollison(bool State)
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = State;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = State;
    }
}
