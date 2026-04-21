using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class textTestScript : MonoBehaviour
{
    [Header("Displaying text")]
    public GameObject textCanvus;
    public Text testMesh;
    public Text speakerNameTextBox;
    public float timePerCharacter = 0.2f;
    [Header("Dialuge scene with all the text files")]
    public textDisplayManager testText;

    [SerializeField] textWriter textWriterScript;

    private void Start()
    {
        if (textWriterScript == null)
        {
            Debug.LogError($"required reference {textWriterScript}");
            this.enabled = false;
        }
        else if (testText == null)
        {
            this.enabled = false;
            Debug.LogError($"required reference text to display");
        }

        textCanvus.SetActive(false);
    }

    [ContextMenu("Write Text")]
    public void writeText()
    {
        StartCoroutine(displayText());
    }

    IEnumerator displayText()
    {
        // show the canvus and all the children
        textCanvus.SetActive(true);
        foreach (textDialogue text in testText.textDialogues)
        {
            textWriterScript.addWriter(testMesh, speakerNameTextBox, text.Dialague, text.nameOfSpeaker, timePerCharacter, true);
            yield return textWriterScript.writeText();
        }
        // deactivate the canvus and all the children
        textCanvus.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
            return;
        writeText();
        GetComponent<BoxCollider>().enabled = false;
    }
}
