using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class PowerBoxEndCollsion : MonoBehaviour
{
    public List<Vector3> RandomPos = new List<Vector3>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    private void OnTriggerEnter(Collider other)
    {
        print("You win");

    }
    public void SetPos(Transform ParentTransform)
    {
        int num = Random.Range(0, RandomPos.Count);
        print(num);
        transform.localPosition = RandomPos[num];
    }
}
