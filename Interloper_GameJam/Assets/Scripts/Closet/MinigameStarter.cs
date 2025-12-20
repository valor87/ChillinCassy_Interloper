using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class MinigameStarter : MonoBehaviour
{
    public PlayerHidingInCloset closet;
    public List<GameObject> interlopers;

    public bool minigameAlreadyPlayed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (interlopers.Count > 0 && !minigameAlreadyPlayed)
        {
            closet.RunGame = true;
            minigameAlreadyPlayed = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("something collided");
        GameObject collidedObj = collision.gameObject;
        if (collidedObj.GetComponentInParent<Interloper>() != null)
        {
            interlopers.Add(collidedObj.transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("something entered");
        GameObject collidedObj = other.gameObject;
        if (collidedObj.GetComponentInParent<Interloper>() != null)
        {
            interlopers.Add(collidedObj.transform.parent.gameObject);
        }
    }
}
