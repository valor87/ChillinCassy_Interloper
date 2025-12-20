using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class FogHandler : MonoBehaviour
{
    public ParticleSystem fog;
    public Flashlight flashlight;

    bool noSanity;
    bool noPower;

    EventCore eventCore;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventCore.enableFog.AddListener(EnablingFog);
        eventCore.disableFog.AddListener(DisablingFog);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnablingFog(string condition)
    {
        if (condition == "sanity")
        {
            noSanity = true;
        }
        else if (condition == "power")
        {
            noPower = true;
        }

        if (noSanity || noPower)
        {
            fog.Play();
            flashlight.rayLength /= 2;
        }
    }

    void DisablingFog(string condition)
    {
        if (condition == "sanity")
        {
            noSanity = false;
        }
        else if (condition == "power")
        {
            noPower = false;
        }

        if (!noSanity && !noPower)
        {
            fog.Stop();
            flashlight.rayLength *= 2;
        }
    }
}
