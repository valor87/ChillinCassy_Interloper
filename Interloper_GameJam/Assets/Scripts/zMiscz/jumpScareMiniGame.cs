using UnityEngine;
using UnityEngine.Rendering;

public class jumpScareMiniGame : MonoBehaviour
{
    [Header("Timer propertys")]
    public float time;
    public float currentTime;

    EventCore eventCore;

    private void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        currentTime += Time.deltaTime;

        if (time < currentTime) {
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.gameObject.CompareTag("Player"))
            return;
        eventCore.death.Invoke("Interloper");
    }
}
