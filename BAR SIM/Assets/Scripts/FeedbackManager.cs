using UnityEngine;
using TMPro;

public class FeedbackManager : MonoBehaviour
{
    [Header("Customer Feedback")]
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private TMP_Text feedbackText;

    [Header("Brad Training")]
    [SerializeField] private GameObject bradTrainingPanel;
    [SerializeField] private TMP_Text bradTrainingText;

    [Header("Overall Feedback")]
    [SerializeField] private GameObject generalFeedbackPanel;
    [SerializeField] private TMP_Text generalFeedbackText;

    [Header("Systems")]
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private BartenderInteraction bartenderInteraction;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ResearchDataManager researchDataManager;

    private NPCController currentNPC;

    private int bradListeningScore;
    private int bradFinalMood;

    private int customer2ListeningScore;
    private int customer2FinalMood;

    private void Awake()
    {
        SetPanel(feedbackPanel, false);
        SetPanel(bradTrainingPanel, false);
        SetPanel(generalFeedbackPanel, false);
    }

    // Resets feedback data
    public void ResetFeedbackData()
    {
        bradListeningScore = 0;
        bradFinalMood = 0;

        customer2ListeningScore = 0;
        customer2FinalMood = 0;

        currentNPC = null;

        SetPanel(feedbackPanel, false);
        SetPanel(bradTrainingPanel, false);
        SetPanel(generalFeedbackPanel, false);
    }

    // Shows individual customer feedback
    public void ShowFeedback(
        NPCController npc,
        int finalMood,
        int listeningScore)
    {
        if (npc == null)
            return;

        currentNPC = npc;

        int customerIndex =
            gameManager.CurrentCustomerIndex;

        if (researchDataManager != null)
        {
            researchDataManager.RecordCustomerResult(
                customerIndex,
                npc.NPCName,
                listeningScore,
                finalMood
            );
        }

        if (customerIndex == 0)
        {
            bradListeningScore =
                listeningScore;

            bradFinalMood =
                finalMood;

            if (feedbackText != null)
            {
                feedbackText.text =
                    BuildBradFeedback(
                        listeningScore,
                        finalMood
                    );
            }
        }
        else
        {
            customer2ListeningScore =
                listeningScore;

            customer2FinalMood =
                finalMood;

            if (feedbackText != null)
            {
                feedbackText.text =
                    BuildCustomer2Feedback(
                        listeningScore,
                        finalMood
                    );
            }
        }

        SetPanel(feedbackPanel, true);
    }

    // Builds Brad's formative feedback
    private string BuildBradFeedback(
        int listeningScore,
        int finalMood)
    {
        string listeningFeedback;

        if (listeningScore >= 4)
        {
            listeningFeedback =
                "Active Listening: Strong\n\n" +
                "You demonstrated active listening in " +
                listeningScore +
                " of 5 opportunities.\n\n" +
                "You regularly reflected Brad's concerns " +
                "and encouraged him to explain further.";
        }
        else if (listeningScore >= 2)
        {
            listeningFeedback =
                "Active Listening: Developing\n\n" +
                "You demonstrated active listening in " +
                listeningScore +
                " of 5 opportunities.\n\n" +
                "You used active listening at some points, " +
                "but missed opportunities to reflect " +
                "what Brad was communicating.";
        }
        else
        {
            listeningFeedback =
                "Active Listening: Limited\n\n" +
                "You demonstrated active listening in " +
                listeningScore +
                " of 5 opportunities.\n\n" +
                "Try reflecting what the customer is " +
                "communicating and allowing them to " +
                "explain further before moving towards " +
                "a solution.";
        }

        return listeningFeedback +
               "\n\n" +
               BuildMoodFeedback(finalMood);
    }

    // Builds Customer 2 feedback
    private string BuildCustomer2Feedback(
        int listeningScore,
        int finalMood)
    {
        string listeningFeedback;

        if (listeningScore >= 4)
        {
            listeningFeedback =
                "Active Listening: Strong\n\n" +
                "You demonstrated active listening in " +
                listeningScore +
                " of 5 opportunities.";
        }
        else if (listeningScore >= 2)
        {
            listeningFeedback =
                "Active Listening: Developing\n\n" +
                "You demonstrated active listening in " +
                listeningScore +
                " of 5 opportunities.\n\n" +
                "Some opportunities to reflect or " +
                "explore the customer's concerns " +
                "were missed.";
        }
        else
        {
            listeningFeedback =
                "Active Listening: Limited\n\n" +
                "You demonstrated active listening in " +
                listeningScore +
                " of 5 opportunities.";
        }

        return listeningFeedback +
               "\n\n" +
               BuildMoodFeedback(finalMood);
    }

    // Builds mood feedback
    private string BuildMoodFeedback(
        int finalMood)
    {
        if (finalMood > 0)
        {
            return
                "Customer Mood: Improved\n\n" +
                "The customer's emotional state " +
                "improved during the interaction.";
        }

        if (finalMood == 0)
        {
            return
                "Customer Mood: Stable\n\n" +
                "The customer's emotional state " +
                "remained relatively stable.";
        }

        return
            "Customer Mood: Still Distressed\n\n" +
            "The customer remained emotionally " +
            "distressed at the end of the interaction.";
    }

    // Continues from individual feedback
    public void ContinueFromCustomerFeedback()
    {
        SetPanel(feedbackPanel, false);

        if (gameManager.CurrentCustomerIndex == 0)
        {
            ShowBradTraining();
        }
        else
        {
            SendCurrentCustomerOut();
        }
    }

    // Shows active-listening lesson after Brad
    private void ShowBradTraining()
    {
        if (bradTrainingText != null)
        {
            bradTrainingText.text =
                "Active Listening Training\n\n" +

                "Before the next customer, remember:\n\n" +

                "Listen to what the customer is actually " +
                "communicating before deciding how to respond.\n\n" +

                "Reflect or paraphrase their concern when " +
                "appropriate to show that you understand.\n\n" +

                "Give the customer opportunities to explain " +
                "or elaborate on what they are experiencing.\n\n" +

                "Avoid rushing to give advice or solve the " +
                "problem before understanding what the " +
                "customer needs.\n\n" +

                "Apply these ideas in the next interaction.";
        }

        SetPanel(
            bradTrainingPanel,
            true
        );
    }

    // Continues after Brad's training lesson
    public void ContinueAfterBradTraining()
    {
        SetPanel(
            bradTrainingPanel,
            false
        );

        SendCurrentCustomerOut();
    }

    // Sends current NPC out
    private void SendCurrentCustomerOut()
    {
        if (currentNPC == null)
            return;

        NPCController leavingNPC =
            currentNPC;

        currentNPC = null;

        if (cameraManager != null)
        {
            cameraManager.FollowNPC(
                leavingNPC
            );
        }

        if (bartenderInteraction != null)
        {
            bartenderInteraction
                .EndNPCInteraction();
        }

        leavingNPC.LeaveBar();
    }

    // Shows overall feedback
    public void ShowGeneralFeedback()
    {
        string comparison;

        if (customer2ListeningScore >
            bradListeningScore)
        {
            comparison =
                "You used more active-listening " +
                "responses in the second interaction.";
        }
        else if (customer2ListeningScore ==
                 bradListeningScore)
        {
            comparison =
                "Your active-listening score was " +
                "the same across both interactions.";
        }
        else
        {
            comparison =
                "You used fewer active-listening " +
                "responses in the second interaction.";
        }

        if (generalFeedbackText != null)
        {
            generalFeedbackText.text =
                "Overall Feedback\n\n" +

                "Brad\n" +
                "Active Listening: " +
                bradListeningScore +
                "/5\n" +
                "Final Mood: " +
                MoodName(bradFinalMood) +
                "\n\n" +

                "Customer 2\n" +
                "Active Listening: " +
                customer2ListeningScore +
                "/5\n" +
                "Final Mood: " +
                MoodName(customer2FinalMood) +
                "\n\n" +

                comparison;
        }

        SetPanel(
            generalFeedbackPanel,
            true
        );
    }

    // Continues to post-questionnaire
    public void CloseGeneralFeedback()
    {
        SetPanel(
            generalFeedbackPanel,
            false
        );

        if (gameManager != null)
            gameManager.GameplayCompleted();
    }

    // Converts mood number to text
    private string MoodName(int mood)
    {
        if (mood <= -2)
            return "Very Negative";

        if (mood == -1)
            return "Negative";

        if (mood == 0)
            return "Neutral";

        if (mood == 1)
            return "Improving";

        return "Positive";
    }

    // Changes panel state
    private void SetPanel(
        GameObject panel,
        bool state)
    {
        if (panel != null)
            panel.SetActive(state);
    }
}