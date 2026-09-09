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

    [Header("Systems")]
    [SerializeField] private DrinksSystem drinksSystem;
    [SerializeField] private MoodSystem moodSystem;
    [SerializeField] private FeedbackManager feedbackManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ResearchDataManager researchDataManager;

    private Story currentStory;
    private NPCController currentNPC;

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        HideChoices();
    }

    // Starts NPC dialogue
    public void StartDialogue(NPCController npc)
    {
        if (npc == null)
            return;

        NPCData npcData =
            npc.NPCData;

        if (npcData == null)
        {
            Debug.LogError(
                "NPCData is missing."
            );

            return;
        }

        if (npcData.inkDialogue == null)
        {
            Debug.LogError(
                npcData.npcName +
                " has no Ink JSON assigned."
            );

            return;
        }

        currentNPC = npc;

        currentStory =
            new Story(
                npcData.inkDialogue.text
            );

        if (npcNameText != null)
            npcNameText.text =
                npcData.npcName;

        currentStory.variablesState["mood"] =
            npcData.startingMood;

        currentStory.variablesState["listeningScore"] =
            0;

        currentStory.variablesState["drink_choice"] =
            "";

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        HideChoices();

        ContinueStory();
    }

    // Continues Ink story
    private void ContinueStory()
    {
        if (currentStory == null)
            return;

        while (currentStory.canContinue)
        {
            string line =
                currentStory
                    .Continue()
                    .Trim();

            if (!string.IsNullOrEmpty(line) &&
                dialogueText != null)
            {
                dialogueText.text =
                    line;
            }

            UpdateMoodFromInk();

            if (CheckForDrinkChoice())
            {
                HideChoices();

                if (drinksSystem != null)
                    drinksSystem.ShowDrinkChoices();

                return;
            }

            if (currentStory.currentChoices.Count > 0)
            {
                DisplayChoices();
                return;
            }
        }

        if (!currentStory.canContinue &&
            currentStory.currentChoices.Count == 0)
        {
            FinishConversation();
        }
    }

    // Checks for service choice tag
    private bool CheckForDrinkChoice()
    {
        if (currentStory == null)
            return false;

        foreach (
            string tag
            in currentStory.currentTags)
        {
            if (tag.Trim().Equals(
                "DRINK_CHOICE",
                System.StringComparison
                    .OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    // Displays dialogue choices
    private void DisplayChoices()
    {
        HideChoices();

        if (currentStory == null)
            return;

        var choices =
            currentStory.currentChoices;

        for (int i = 0;
             i < choices.Count;
             i++)
        {
            if (i >= choiceButtons.Length)
                break;

            Button button =
                choiceButtons[i];

            if (button == null)
                continue;

            button.gameObject.SetActive(true);

            TMP_Text buttonText =
                button.GetComponentInChildren<TMP_Text>();

            if (buttonText != null)
            {
                buttonText.text =
                    choices[i].text.Trim();
            }

            int choiceIndex = i;

            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(
                () => SelectChoice(
                    choiceIndex
                )
            );
        }
    }

    // Selects dialogue choice
    private void SelectChoice(
        int choiceIndex)
    {
        if (currentStory == null)
            return;

        if (choiceIndex < 0 ||
            choiceIndex >=
            currentStory.currentChoices.Count)
        {
            return;
        }

        string selectedChoice =
            currentStory
                .currentChoices[choiceIndex]
                .text
                .Trim();

        if (researchDataManager != null &&
            gameManager != null)
        {
            researchDataManager.RecordDialogueChoice(
                gameManager.CurrentCustomerIndex,
                selectedChoice
            );
        }

        currentStory.ChooseChoiceIndex(
            choiceIndex
        );

        HideChoices();

        ContinueStory();
    }

    // Receives service choice
    public void SubmitDrinkChoice(
        string choiceID)
    {
        if (currentStory == null)
            return;

        if (researchDataManager != null &&
            gameManager != null)
        {
            researchDataManager.RecordDrinkChoice(
                gameManager.CurrentCustomerIndex,
                choiceID
            );
        }

        currentStory.variablesState["drink_choice"] =
            choiceID;

        if (drinksSystem != null)
            drinksSystem.HideDrinkChoices();

        ContinueStory();
    }

    // Reads mood from Ink
    private void UpdateMoodFromInk()
    {
        if (currentStory == null ||
            moodSystem == null)
        {
            return;
        }

        object moodValue =
            currentStory.variablesState["mood"];

        if (moodValue == null)
            return;

        moodSystem.SetMood(
            (int)moodValue
        );
    }

    // Hides choice buttons
    private void HideChoices()
    {
        if (choiceButtons == null)
            return;

        foreach (Button button in choiceButtons)
        {
            if (button == null)
                continue;

            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
    }

    // Finishes NPC conversation
    private void FinishConversation()
    {
        if (currentStory == null)
            return;

        int finalMood =
            (int)currentStory
                .variablesState["mood"];

        int listeningScore =
            (int)currentStory
                .variablesState["listeningScore"];

        Debug.Log(
            "Conversation Finished | NPC: " +
            currentNPC.NPCName +
            " | Mood: " +
            finalMood +
            " | Listening Score: " +
            listeningScore
        );

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        HideChoices();

        if (drinksSystem != null)
            drinksSystem.HideDrinkChoices();

        NPCController finishedNPC =
            currentNPC;

        if (feedbackManager != null)
        {
            feedbackManager.ShowFeedback(
                finishedNPC,
                finalMood,
                listeningScore
            );
        }

        currentStory = null;
        currentNPC = null;
    }
}