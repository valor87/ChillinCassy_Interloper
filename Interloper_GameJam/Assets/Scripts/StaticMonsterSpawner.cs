using UnityEngine;

public class StaticMonsterSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject staticMonsterPrefab;
    [Header("Values")]
    //percent chance of spawning an static monster. 0-100
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

    //check if a static monster should be spawned by chance
    void SpawnCheck()
    {
        if (Random.Range(1, 101) <= spawnFreq)
        {
            Vector3 spawnPoint = new Vector3(Random.Range(-7, 27), 2.6f, Random.Range(15, -15));
            if (spawnPoint != null)
            {
                GameObject staticMonsterObj = Instantiate(staticMonsterPrefab, spawnPoint, Quaternion.identity);
                staticMonsterObj.GetComponent<StaticMonster>().killTimer = 20;
            }

        }
    }
}
