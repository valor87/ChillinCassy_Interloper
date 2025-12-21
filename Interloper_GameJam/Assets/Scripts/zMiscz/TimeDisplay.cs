using TMPro;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
    GameManager gameManager;

    TextMeshProUGUI timeLeftText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        timeLeftText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        updateDisplay();
    }

    void updateDisplay()
    {
        float timeLeft = 300 - gameManager.gameTimer;
        timeLeftText.text = "Time Left: " + timeLeft;
    }
}
