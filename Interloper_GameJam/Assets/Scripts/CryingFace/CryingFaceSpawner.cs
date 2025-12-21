using UnityEngine;

public class CryingFaceSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject cryingFacePrefab;
    public Transform PlayerTransform;
    [Header("Values")]
    //percent chance of spawning an crying face. 0-100
    public float spawnFreq;
    //amount of time needed for a bookshelf check to happen. in seconds
    public float spawnTimeWindow;
    public Vector3 lowerSpawnBound;
    public Vector3 upperSpawnBound;

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
            SpawnCryingFace();
        }
    }

    public void SpawnCryingFace()
    {
        Vector3 spawnPoint;
        if (lowerSpawnBound != Vector3.zero && upperSpawnBound != Vector3.zero) 
        {
            spawnPoint = new Vector3(Random.Range(lowerSpawnBound.x, upperSpawnBound.x), Random.Range(lowerSpawnBound.y, upperSpawnBound.y), Random.Range(lowerSpawnBound.z, upperSpawnBound.z));
        }
        else
        {
            spawnPoint = new Vector3(Random.Range(3, -7), 2.6f, Random.Range(15, -13));
        }
        
        if (spawnPoint != null)
        {
            GameObject cryingFaceObj = Instantiate(cryingFacePrefab, spawnPoint, Quaternion.identity);
            cryingFaceObj.transform.parent = transform;
            cryingFaceObj.GetComponent<CryingFace>().killTimer = 20;
            cryingFaceObj.GetComponent<CryingFace>().Player = PlayerTransform;
        }
    }
}
