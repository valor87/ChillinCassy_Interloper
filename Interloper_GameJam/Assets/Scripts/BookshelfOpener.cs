using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BookshelfOpener : MonoBehaviour
{
    //percent chance of opening a bookshelf. 0-100
    public float openFreq;
    //amount of time needed for a bookshelf check to happen. in seconds
    public float timeWindow;
    public GameObject[] bookshelves;

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

        if (timer > timeWindow)
        {
            BookshelfCheck();
            timer = 0;
        }
    }

    //check if a bookshelf should be opened by chance
    void BookshelfCheck()
    {
        if (Random.Range(1, 101) <= openFreq)
        {
            GameObject bookshelf = PickBookshelf();
            if (bookshelf != null)
                eventCore.unblockBookshelf.Invoke(bookshelf);
        }
    }

    //picks a closed bookshelf. returns null if all bookshelves are open
    GameObject PickBookshelf()
    {
        List<GameObject> tempBookshelves = new List<GameObject>(bookshelves);
        

        while (tempBookshelves.Count > 0)
        {
            GameObject bookshelf = tempBookshelves[Random.Range(0, tempBookshelves.Count)];
            if (bookshelf.GetComponent<Bookshelf>().activelyBlocking)
            {
                return bookshelf;
            }
            tempBookshelves.Remove(bookshelf);
        }
        return null;
    }
}
