using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class PowerBoxEndCollsion : MonoBehaviour
{
    public Transform PowerBox;
    EventCore EventCore;
    public PowerBoxScript PowerBoxScript;
    public List<Vector3> RandomPos = new List<Vector3>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        EventCore.enableFog.AddListener(SetPos);
    }
    private void OnTriggerEnter(Collider other)
    {
        print("You win");
        PowerBoxScript.runrepair = false;
    }
    public void SetPos(string Cause)
    {
        if (Cause == "power")
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
            int num = Random.Range(0, RandomPos.Count);
            transform.localPosition = RandomPos[num];
        }
    }
}
