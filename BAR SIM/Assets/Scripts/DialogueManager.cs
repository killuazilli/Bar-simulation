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

    private Story currentStory;
    private NPCController currentNPC;

    private void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        HideChoices();
    }

    public void StartDialogue(NPCController npc)
    {
        if (npc == null)
            return;

        NPCData npcData = npc.NPCData;

        if (npcData == null)
        {
            Debug.LogError(
                "NPCData is missing on " +
                npc.gameObject.name
            );

            return;
        }

        if (npcData.inkDialogue == null)
        {
            Debug.LogError(
                npcData.npcName +
                " has no Ink dialogue file assigned."
            );

            return;
        }

        currentNPC = npc;

        // Create the Ink story belonging
        // to the current NPC.
        currentStory =
            new Story(npcData.inkDialogue.text);

        if (npcNameText != null)
        {
            npcNameText.text =
                npcData.npcName;
        }

        // Starting values are passed
        // from Unity into Ink.
        currentStory.variablesState["mood"] =
            npcData.startingMood;

        currentStory.variablesState["listeningScore"] =
            0;

        currentStory.variablesState["drink_choice"] =
            "";

        // MoodSystem displays the
        // NPC's starting emotional state.
        if (moodSystem != null)
        {
            moodSystem.SetMood(
                npcData.startingMood
            );
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentStory == null)
            return;

        // If Ink has another line,
        // display it.
        if (currentStory.canContinue)
        {
            string line =
                currentStory.Continue().Trim();

            if (dialogueText != null)
            {
                dialogueText.text = line;
            }

            // Read any mood change
            // made inside Ink.
            UpdateMood();

            // Check tags attached
            // to the current Ink line.
            foreach (
                string tag
                in currentStory.currentTags)
            {
                if (
                    tag.Trim().Equals(
                        "DRINK_CHOICE",
                        System.StringComparison
                            .OrdinalIgnoreCase
                    )
                )
                {
                    HideChoices();

                    if (drinksSystem != null)
                    {
                        drinksSystem
                            .ShowDrinkChoices();
                    }

                    return;
                }
            }

            // If Ink has produced dialogue
            // choices, display them.
            if (
                currentStory
                    .currentChoices.Count > 0)
            {
                DisplayChoices();
                return;
            }

            /*
             * If there are no choices but Ink
             * still contains more story content,
             * continue to the next line.
             *
             * This is useful for consecutive
             * NPC dialogue lines.
             */
            if (currentStory.canContinue)
            {
                ContinueStory();
                return;
            }
        }

        // If there is nothing else
        // to continue and no choices remain,
        // the interaction is complete.
        if (
            currentStory != null &&
            !currentStory.canContinue &&
            currentStory.currentChoices.Count == 0)
        {
            FinishConversation();
        }
    }

    private void DisplayChoices()
    {
        HideChoices();

        if (currentStory == null)
            return;

        var choices =
            currentStory.currentChoices;

        for (
            int i = 0;
            i < choices.Count;
            i++)
        {
            if (i >= choiceButtons.Length)
            {
                Debug.LogWarning(
                    "Ink contains more choices " +
                    "than the Dialogue UI has buttons."
                );

                break;
            }

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
                () =>
                    SelectChoice(
                        choiceIndex
                    )
            );
        }
    }

    private void SelectChoice(
        int choiceIndex)
    {
        if (currentStory == null)
            return;

        currentStory.ChooseChoiceIndex(
            choiceIndex
        );

        HideChoices();

        ContinueStory();
    }

    public void SubmitDrinkChoice(
        string choiceID)
    {
        if (currentStory == null)
            return;

        /*
         * DrinksSystem does not alter mood.
         *
         * It only tells Ink which service
         * choice the player selected.
         */
        currentStory.variablesState[
            "drink_choice"
        ] = choiceID;

        ContinueStory();
    }

    private void UpdateMood()
    {
        if (
            currentStory == null ||
            moodSystem == null)
        {
            return;
        }

        object moodValue =
            currentStory.variablesState[
                "mood"
            ];

        if (moodValue == null)
            return;

        int mood =
            (int)moodValue;

        // MoodSystem displays the value.
        // It does not calculate the result.
        moodSystem.SetMood(mood);
    }

    private void HideChoices()
    {
        if (choiceButtons == null)
            return;

        foreach (
            Button button
            in choiceButtons)
        {
            if (button == null)
                continue;

            button.gameObject.SetActive(false);

            button.onClick.RemoveAllListeners();
        }
    }

    private void FinishConversation()
    {
        if (currentStory == null)
            return;

        int finalMood =
            (int)currentStory
                .variablesState["mood"];

        int listeningScore =
            (int)currentStory
                .variablesState[
                    "listeningScore"
                ];

        // Close conversation UI.
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        HideChoices();

        if (drinksSystem != null)
        {
            drinksSystem.HideDrinkChoices();
        }

        /*
         * Send the complete interaction
         * results to FeedbackManager.
         *
         * FeedbackManager keeps its own
         * reference to the NPC so it can
         * make the NPC leave afterwards.
         */
        if (feedbackManager != null)
        {
            feedbackManager.ShowFeedback(
                currentNPC,
                finalMood,
                listeningScore
            );
        }

        // DialogueManager no longer needs
        // to hold the completed conversation.
        currentStory = null;
        currentNPC = null;
    }
}