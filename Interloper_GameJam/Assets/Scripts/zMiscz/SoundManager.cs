using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    EventCore eventcore;
    public Settings settings;
    public AudioClip InterloperKillSound;
    AudioSource AS;
    void Start()
    {
        eventcore = GameObject.Find("EventCore").GetComponent<EventCore>();
        eventcore.death.AddListener(deathSound);
        AS = GetComponent<AudioSource>();
        AS.volume = settings.ambienceVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PLayOneShot(AudioClip AC)
    {
        AS.PlayOneShot(AC, settings.ambienceVolume);
    }
    void deathSound(string cause)
    {
        if (cause == "Interloper")
        {
            AS.PlayOneShot(InterloperKillSound);
        }
    }
}
