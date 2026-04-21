using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class textWriter : MonoBehaviour
{
    [Header("For animating the confirm image")]
    [Tooltip("Must have an animator and a confirm animation to play")]
    [SerializeField] string animBoolName = "confirmAnim";
    public GameObject animationConfirm;
    [Header("Key used for confirming the text")]
    [SerializeField] bool getkeyInput = true;
    [SerializeField] KeyCode confirmKey = KeyCode.Space;

    [Header("Not using key input to confirm")]
    [SerializeField] float timeTillNextTextBox;

    private Text uiText;
    private Text speakerNameTextBox;
    private string textToWrite;
    private string speakerName;
    private int characterIndex;
    private float timePerCharacter;
    private float timer;
    private bool invisableCharacter;
    [HideInInspector]
    public bool doneDisplaying;

    private void Start()
    {
        animationConfirm.SetActive(false);

        if (animationConfirm.GetComponent<Animator>() == null)
        {
            Debug.LogError("Could not find an animator on the confirm image game object.");
        }
    }

    public void addWriter(Text uiText, Text speakerNameTextBox, string textToWrite, string speakerName, float timerPerCharacter, bool invisableCharacter)
    {
        this.uiText = uiText;
        this.speakerNameTextBox = speakerNameTextBox;
        this.textToWrite = textToWrite;
        this.speakerName = speakerName;
        this.timePerCharacter = timerPerCharacter;
        this.invisableCharacter = invisableCharacter;
    }

    public IEnumerator writeText()
    {
        bool textWriting = true;
        float timerPerLetter = timePerCharacter;
        speakerNameTextBox.text = speakerName;
        while (textWriting)
        {
            if (uiText != null)
            {
                if (Input.GetKeyDown(confirmKey) && getkeyInput)
                {
                    timerPerLetter = 0.01f;
                }

                timer -= Time.deltaTime;
                while (timer < 0f)
                {
                    // display next character
                    timer += timerPerLetter;
                    characterIndex++;

                    string text = textToWrite.Substring(0, characterIndex);
                    if (invisableCharacter)
                    {
                        text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
                    }

                    uiText.text = text;

                    if (characterIndex >= textToWrite.Length)
                    {
                        textWriting = false;
                        characterIndex = 0;
                    }
                }
            }
            yield return null;
        }
        bool continueText = false;
        // turn both the confirm image on an turn the animation on
        setAnimator(animationConfirm, animBoolName, true);

        if (getkeyInput) {
            while (!continueText)
            {
                if (Input.GetKeyDown(confirmKey))
                {
                    continueText = true;
                }
                yield return null;
            }
        }
        else
        {
            setAnimator(animationConfirm, animBoolName, false);
            yield return new WaitForSeconds(timeTillNextTextBox);
        }
        // turn both the confirm image off an turn the animation off
        setAnimator(animationConfirm, animBoolName, false);
        uiText.text = "";
    }

    void setAnimator(GameObject animatorObject, string animationName, bool State)
    {
        animatorObject.SetActive(State);
        animationConfirm.GetComponent<Animator>().SetBool(animationName, State);
    }

}
