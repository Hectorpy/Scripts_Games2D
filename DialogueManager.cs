using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private int pontosTotal = 0;
    public TextMeshProUGUI pontosText;

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    private GameObject currentNPC; // Referência ao NPC atual

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            Time.timeScale = 1;
            return;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON, GameObject npc)
    {
        currentNPC = npc; // Associe o NPC atual

        if (currentStory != null)
        {
            currentStory.ResetState();
        }
        else
        {
            currentStory = new Story(inkJSON.text);
        }
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        NPCEntrance npcEntrance = currentNPC.GetComponent<NPCEntrance>();
        if (npcEntrance != null)
        {
            npcEntrance.ExitRoom();
        }
    }

    private void ContinueStory()
    {
        if (currentStory != null)
        {
            if (currentStory.canContinue)
            {
                string storyText = currentStory.Continue();
                if (!string.IsNullOrEmpty(storyText))
                {
                    dialogueText.text = storyText;
                    pontosTotal = (int)currentStory.variablesState["pontosTotal"];
                    pontosText.text = "Pontos: " + pontosTotal.ToString();
                    DisplayChoices();
                }
                else
                {
                    ExitDialogueMode();
                }
            }
            else
            {
                ExitDialogueMode();
            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("Foram dadas mais escolhas do que a UI pode suportar. Numero de escolhas dadas: " + currentChoices.Count);
        }

        int index = 0;

        foreach (var choiceUI in choices)
        {
            choiceUI.gameObject.SetActive(true);
        }

        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        ContinueStory();
        currentStory.ChooseChoiceIndex(choiceIndex);
    }

    public void SairDialogo(int choiceIndex)
    {
        ExitDialogueMode();
        currentStory.ChooseChoiceIndex(choiceIndex);
    }

    public int GetpontosTotal()
    {
        return pontosTotal;
    }
}