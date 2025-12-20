using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Values")]
    public float gameLength = 300f; //in seconds
    
    InterloperSpawner interloperSpawner;
    CryingFaceSpawner cryingFaceSpawner;
    PowerHandler powerHandler;
    SanityHandler sanityHandler;
    BookshelfOpener bookshelfOpener;

    EventCore eventCore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interloperSpawner = GameObject.Find("InterloperSpawner").GetComponent<InterloperSpawner>();
        cryingFaceSpawner = GameObject.Find("CryingFaceSpawner").GetComponent<CryingFaceSpawner>();
        powerHandler = GameObject.Find("PowerHandler").GetComponent<PowerHandler>();
        sanityHandler = GameObject.Find("SanityHandler").GetComponent<SanityHandler>();
        bookshelfOpener = GameObject.Find("BookshelfOpener").GetComponent<BookshelfOpener>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
