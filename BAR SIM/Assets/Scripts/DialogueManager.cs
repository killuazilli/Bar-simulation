using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private Button[] choiceButtons;

    [Header("Systems")]
    [SerializeField] private MoodSystem moodSystem;

    private NPCData currentNPC;

    public void StartDialogue(NPCData npc)
    {
        currentNPC = npc;

        dialoguePanel.SetActive(true);

        npcNameText.text = npc.npcName;
        dialogueText.text = npc.openingDialogue;

        moodSystem.SetMood(npc.startingMood);
    }

    public void DisplayChoices(List<DialogueChoice> choices)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Count)
            {
                choiceButtons[i].gameObject.SetActive(true);

                DialogueChoice choice = choices[i];

                choiceButtons[i]
                    .GetComponentInChildren<TMP_Text>()
                    .text = choice.choiceText;

                choiceButtons[i].onClick.RemoveAllListeners();

                choiceButtons[i].onClick.AddListener(
                    () => SelectChoice(choice)
                );
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void SelectChoice(DialogueChoice choice)
    {
        dialogueText.text = choice.npcResponse;

        moodSystem.ChangeMood(choice.moodEffect);

        HideChoices();
    }

    private void HideChoices()
    {
        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentNPC = null;
    }
}