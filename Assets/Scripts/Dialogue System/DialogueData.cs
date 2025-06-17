using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Game/Dialogue")]
public class DialogueData : ScriptableObject
{
    public string characterName;
    public List<string> sentences;
    public float typingSpeed = 0.05f;
    public string dialogueID;

    [System.Serializable]
    public class DialogueOption
    {
        public string optionText;
        public DialogueData nextDialogue;
    }

    public List<DialogueOption> options = new List<DialogueOption>();
}
