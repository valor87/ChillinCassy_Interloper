using UnityEngine;

public class CryingFaceSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject cryingFacePrefab;
    [Header("Values")]
    //percent chance of spawning an crying face. 0-100
    public float spawnFreq;
    //amount of time needed for a bookshelf check to happen. in seconds
    public float spawnTimeWindow;

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

        if (timer > spawnTimeWindow)
        {
            SpawnCheck();
            timer = 0;
        }
    }

    //check if a crying face should be spawned by chance
    void SpawnCheck()
    {
        if (Random.Range(1, 101) <= spawnFreq)
        {
            Vector3 spawnPoint = new Vector3(Random.Range(-7, 27), 2.6f, Random.Range(15, -15));
            if (spawnPoint != null)
            {
                GameObject cryingFaceObj = Instantiate(cryingFacePrefab, spawnPoint, Quaternion.identity);
                cryingFaceObj.GetComponent<CryingFace>().killTimer = 20;
            }

        }
    }
}
