using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Choice Buttons")]
    [SerializeField] private Button[] choiceButtons;

    private Story currentStory;
    private NPCController currentNPC;

    private void Start()
    {
        dialoguePanel.SetActive(false);

        HideChoices();
    }

    // Called by BartenderInteraction
    public void StartDialogue(NPCController npc)
    {
        if (npc == null)
            return;

        currentNPC = npc;

        NPCData data = currentNPC.NPCData;

        if (data == null)
        {
            Debug.LogError("NPC has no NPCData assigned.");
            return;
        }

        if (data.inkDialogue == null)
        {
            Debug.LogError("NPC has no Ink dialogue assigned.");
            return;
        }

        currentStory = new Story(data.inkDialogue.text);

        npcNameText.text = data.npcName;

        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentStory == null)
            return;

        // NPC still has dialogue
        if (currentStory.canContinue)
        {
            string dialogue = currentStory.Continue();

            dialogueText.text = dialogue.Trim();

            DisplayChoices();

            return;
        }

        // Story has finished
        if (currentStory.currentChoices.Count == 0)
        {
            EndDialogue();
        }
    }

    private void DisplayChoices()
    {
        HideChoices();

        var choices = currentStory.currentChoices;

        for (int i = 0; i < choices.Count; i++)
        {
            if (i >= choiceButtons.Length)
                break;

            Button button = choiceButtons[i];

            button.gameObject.SetActive(true);

            TMP_Text buttonText =
                button.GetComponentInChildren<TMP_Text>();

            buttonText.text = choices[i].text.Trim();

            int choiceIndex = i;

            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(
                () => SelectChoice(choiceIndex)
            );
        }
    }

    private void SelectChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);

        HideChoices();

        ContinueStory();
    }

    private void HideChoices()
    {
        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false);

            button.onClick.RemoveAllListeners();
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        HideChoices();

        currentStory = null;

        currentNPC = null;
    }
}