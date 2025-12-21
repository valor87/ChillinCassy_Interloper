using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class FloodLightManager : MonoBehaviour
{
    public List<GameObject> Lights;
    EventCore eventcore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventcore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventcore.enableFog.AddListener(TurnoffLights);
        eventcore.disableFog.AddListener(TurnonLights);
    }

    void TurnoffLights(string cause)
    {
        foreach (var light in Lights)
        {
            light.SetActive(false);
        }
    }
    void TurnonLights(string cause)
    {
        foreach (var light in Lights)
        {
            light.SetActive(true);
        }
    }
}
