using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("Dialogue UI")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI dialogueTextBox;
    [SerializeField] private TextMeshProUGUI nameTextBox;

    [Header("Options UI")]
    [SerializeField] private GameObject pressToContinueText;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private List<Button> optionButtons; // UnityEngine.UI
    [SerializeField] private List<TextMeshProUGUI> optionTexts;

    private bool isTyping = false;
    private string currentSentence = "";
    private Coroutine typingCoroutine;
    private Queue<string> sentences = new Queue<string>();
    private float typingSpeed = 0.05f;
    private DialogueData currentData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            HideDialogueUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartDialogue(DialogueData data)
    {
        ShowDialogueUI();
        currentData = data;
        nameTextBox.text = data.characterName;
        typingSpeed = data.typingSpeed;

        sentences.Clear();
        foreach (string sentence in data.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            // Completa la oración en pantalla sin animación
            StopCoroutine(typingCoroutine);
            dialogueTextBox.text = currentSentence;
            isTyping = false;
            pressToContinueText.SetActive(true);
            return;
        }

        if (sentences.Count == 0)
        {
            if (currentData.options != null && currentData.options.Count > 0)
            {
                ShowOptions(currentData);
            }
            else
            {
                EndDialogue();
            }
            return;
        }

        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }



    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        pressToContinueText.SetActive(false);
        dialogueTextBox.text = "";

        foreach (char letter in sentence)
        {
            dialogueTextBox.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        pressToContinueText.SetActive(true);
    }


    private void ShowOptions(DialogueData data)
    {
        optionsPanel.SetActive(true);
        pressToContinueText.SetActive(false);

        for (int i = 0; i < optionButtons.Count; i++)
        {
            if (i < data.options.Count)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionTexts[i].text = data.options[i].optionText;

                int capturedIndex = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() =>
                {
                    HideOptions();
                    StartDialogue(data.options[capturedIndex].nextDialogue);
                });
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void HideOptions()
    {
        optionsPanel.SetActive(false);
        pressToContinueText.SetActive(true);
        foreach (var button in optionButtons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    public void EndDialogue()
    {
        dialogueTextBox.text = "";
        nameTextBox.text = "";
        HideOptions();
        HideDialogueUI();
    }

    public void ShowDialogueUI()
    {
        canvas.enabled = true;
    }

    public void HideDialogueUI()
    {
        canvas.enabled = false;
    }
    public bool IsDialogueActive()
    {
        return canvas.enabled;
    }
    private void Update()
    {
        if (canvas.enabled && optionsPanel.activeSelf == false && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

}