using System.Collections;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Values")]
    public int gameLength = 300; //in seconds
    //based on how much bookshelves are open, sanity will decrease faster passively. this affects the strength of that penalty
    public int bookshelfSanityPenalty; 
    
    InterloperSpawner interloperSpawner;
    CryingFaceSpawner cryingFaceSpawner;
    PowerHandler powerHandler;
    SanityHandler sanityHandler;
    BookshelfOpener bookshelfOpener;

    EventCore eventCore;

    int gameTimer;

    Coroutine game;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        interloperSpawner = GameObject.Find("InterloperSpawner").GetComponent<InterloperSpawner>();
        cryingFaceSpawner = GameObject.Find("CryingFaceSpawner").GetComponent<CryingFaceSpawner>();
        powerHandler = GameObject.Find("PowerHandler").GetComponent<PowerHandler>();
        sanityHandler = GameObject.Find("SanityHandler").GetComponent<SanityHandler>();
        bookshelfOpener = GameObject.Find("BookshelfOpener").GetComponent<BookshelfOpener>();

        setupGame();
    }

    // Update is called once per frame
    void Update()
    {
        changeSanityRate();
    }

    void setupGame()
    {
        gameTimer = 0;
        interloperSpawner.spawnFreq = 30;
        interloperSpawner.spawnTimeWindow = 10;

        cryingFaceSpawner.spawnFreq = 0;
        cryingFaceSpawner.spawnTimeWindow = 5;

        powerHandler.powerBreakFreq = 0;
        powerHandler.powerBreakTimeWindow = 5;

        sanityHandler.sanityDecreaseRate = 0.5f;

        bookshelfOpener.openFreq = 30;
        bookshelfOpener.timeWindow = 5;

        game = StartCoroutine(ProcessGameEvents());
    }

    //here, all the game events will be checked based on time and happen if conditions are met.
    //that includes the changing of event frequencies or force spawning monsters
    IEnumerator ProcessGameEvents()
    {
        while (gameTimer <= gameLength)
        {
            gameTimer++;

            //force a power break to introduce power to player
            //allow power to start breaking on its own
            //increase chance of bookshelf opening
            //crying faces have a small chance to spawn
            if (gameTimer == 50)
            {
                print("game timer: " + gameTimer);
                powerHandler.PowerBreakCheck(true);
                powerHandler.powerBreakFreq = 50;
                powerHandler.powerBreakTimeWindow = 40;

                bookshelfOpener.openFreq = 50;

                cryingFaceSpawner.spawnFreq = 25;
                cryingFaceSpawner.spawnTimeWindow = 7;
            }
            //forcibly spawn a crying face and increase their spawn rate
            //increase the spawn rate of interlopers
            else if (gameTimer == 100)
            {
                print("game timer: " + gameTimer);
                cryingFaceSpawner.SpawnCryingFace();
                cryingFaceSpawner.spawnFreq = 40;
                cryingFaceSpawner.spawnTimeWindow = 6;
                
                interloperSpawner.spawnFreq = 40;
                interloperSpawner.spawnTimeWindow = 9;
            }
            //force all bookshelves to open
            //increase the rate of passive sanity loss
            //increase rate of power loss
            else if (gameTimer == 150)
            {
                print("game timer: " + gameTimer);
                bookshelfOpener.BookshelfCheck(true);
                bookshelfOpener.BookshelfCheck(true);

                bookshelfSanityPenalty *= 2;

                powerHandler.powerBreakFreq = 75;
                powerHandler.powerBreakTimeWindow = 35;
            }
            //force another power break
            //increase spawn rate of interlopers
            else if (gameTimer == 200)
            {
                print("game timer: " + gameTimer);
                powerHandler.PowerBreakCheck(true);

                interloperSpawner.spawnFreq = 80;
                interloperSpawner.spawnTimeWindow = 7;
            }
            //max out spawn rate of crying face while spacing it out
            //max out rate of unblocking bookshelves while spacing it out
            else if (gameTimer == 250)
            {
                print("game timer: " + gameTimer);
                cryingFaceSpawner.spawnFreq = 100;
                cryingFaceSpawner.spawnTimeWindow = 9;

                bookshelfOpener.openFreq = 100;
                bookshelfOpener.timeWindow = 7;
            }

            yield return new WaitForSeconds(1);
        }

        eventCore.winGame.Invoke();
        print("you win!!");
    }

    //change how fast sanity depletes passively the more bookshelves that are open
    void changeSanityRate()
    {
        sanityHandler.sanityDecreaseRate = 0.5f;
        int multiplier = 0;
        foreach (GameObject bookshelfObj in bookshelfOpener.bookshelves)
        {
            Bookshelf bookshelf = bookshelfObj.GetComponent<Bookshelf>();
            if (!bookshelf.activelyBlocking)
            {
                multiplier += 1;
            }
        }

        sanityHandler.sanityDecreaseRate *= (1f + bookshelfSanityPenalty * multiplier);
    }
}
