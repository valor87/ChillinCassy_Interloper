using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonScript : MonoBehaviour
{
    public GameObject HowToPlay;
    public Settings settings;
    public Slider ambienceVolumeSlider;
    public Slider sfxVolumeSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }
    private void Update()
    {
        settings.ambienceVolume = ambienceVolumeSlider.value;
        settings.sfxVolume = sfxVolumeSlider.value;
    }

    public void PlayGame()
    {
        //game scene
        SceneManager.LoadScene("LevelScene");
    }
}
