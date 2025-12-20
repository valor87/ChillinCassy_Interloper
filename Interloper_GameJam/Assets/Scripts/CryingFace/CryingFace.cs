using System.Linq;
using UnityEngine;

public class CryingFace : MonoBehaviour
{
    public Transform Player;
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
        LookAtPlayer(Player);

        timer += Time.deltaTime;

        if (timer >= killTimer)
        {
            eventCore.death.Invoke("CryingFace");
            timer = 0;
            Destroy(transform.gameObject); 
        }

    }
    void LookAtPlayer(Transform LookAt)
    {
        transform.LookAt(LookAt);
        Vector3 CryingFacerotation = transform.eulerAngles;
        CryingFacerotation.x = 0;
        CryingFacerotation.z = 0;
        transform.eulerAngles = CryingFacerotation;
    }
}
