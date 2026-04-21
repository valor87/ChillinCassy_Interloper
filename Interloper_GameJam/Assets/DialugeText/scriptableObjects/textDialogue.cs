using UnityEngine;

[CreateAssetMenu(fileName = "textDialuge", menuName = "Scriptable Objects/textDialuge")]
public class textDialogue : ScriptableObject
{
    [SerializeField] string textName = "Name of Dialogue";
    public string nameOfSpeaker;
    [TextArea(0, 20)]
    public string Dialague;

}
