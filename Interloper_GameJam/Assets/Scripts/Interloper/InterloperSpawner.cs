using System.Collections.Generic;
using UnityEngine;

public class InterloperSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject interloperPrefab;
    public GameObject[] spawnPoints;
    [Header("Values")]
    //percent chance of spawning an interloper. 0-100
    public float spawnFreq;
    //amount of time needed for a bookshelf check to happen. in seconds
    public float spawnTimeWindow;

    BookshelfOpener bookshelfOpener;

    float timer;
    EventCore eventCore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        bookshelfOpener = GameObject.Find("BookshelfOpener").GetComponent<BookshelfOpener>();
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

    //check if a interloper should be spawned by chance
    void SpawnCheck()
    {
        if (Random.Range(1, 101) <= spawnFreq)
        {
            Transform spawnPoint = PickSpawnPoint();
            if (spawnPoint != null)
            {
                GameObject interloperObj = Instantiate(interloperPrefab, spawnPoint.position, Quaternion.identity);
                interloperObj.GetComponent<Interloper>().returnPoint = spawnPoint;
            }
                
        }
    }

    //picks a spawn point by checking for open bookshelves. if one's open, get the child which is the spawn point
    Transform PickSpawnPoint()
    {
        List<GameObject> tempBookshelves = new List<GameObject>(bookshelfOpener.bookshelves);

        while (tempBookshelves.Count > 0)
        {
            GameObject bookshelf = tempBookshelves[Random.Range(0, tempBookshelves.Count)];
            if (!bookshelf.GetComponent<Bookshelf>().activelyBlocking)
            {
                Transform spawnPoint = bookshelf.transform.GetChild(0);
                return spawnPoint;
            }
            tempBookshelves.Remove(bookshelf);
        }
        return null;
    }

}
