using TMPro;
using UnityEngine;

public class SanityDisplay : MonoBehaviour
{
    SanityHandler sanityHandler;

    TextMeshProUGUI sanityText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sanityHandler = GameObject.Find("SanityHandler").GetComponent<SanityHandler>();
        sanityText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        updateDisplay();
    }

    void updateDisplay()
    {
        float sanity = sanityHandler.sanity;
        sanityText.text = "Sanity: " + (int) sanity;

        if (!(sanity / 100f < (2f / 3f)))
        {
            sanityText.color = Color.green;
        }
        else if (!(sanity / 100f < (1f / 3f)))
        {
            sanityText.color = Color.yellow;
        }
        else
        {
            sanityText.color = Color.red;
        }
    }
}
