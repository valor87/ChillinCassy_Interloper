using UnityEngine;

public class CryingFace : MonoBehaviour
{
    public float killTimer = 20;
    float timer;

    EventCore eventCore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= killTimer)
        {
            eventCore.death.Invoke("CryingFace");
            timer = 0;
            Destroy(transform.gameObject); 
        }

    }
}
