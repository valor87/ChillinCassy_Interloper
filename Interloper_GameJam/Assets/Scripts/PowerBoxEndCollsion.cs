using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class PowerBoxEndCollsion : MonoBehaviour
{
    List<Vector3> RandomPos = new List<Vector3>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision collision)
    {
        print("You win");
    }
    public void SetPos()
    {
        int num = Random.Range(0, RandomPos.Count);
        transform.position = RandomPos[num];
    }
}
