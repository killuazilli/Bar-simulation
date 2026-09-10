using UnityEngine;
using TMPro;

public class FeedbackManager : MonoBehaviour
{
    [Header("Customer Feedback")]
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private TMP_Text feedbackTitleText;
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

    [Header("Scoring")]
    [SerializeField] private int listeningOpportunities = 5;

    private NPCController currentNPC;

    private int bradListeningScore;
    private int bradFinalMood;

    private int angryCustomerListeningScore;
    private int angryCustomerFinalMood;

    private string angryCustomerName = "Customer 2";

    private void Awake()
    {
        SetPanel(feedbackPanel, false);
        SetPanel(bradTrainingPanel, false);
        SetPanel(generalFeedbackPanel, false);
    }

    // Reset feedback data
    public void ResetFeedbackData()
    {
        bradListeningScore = 0;
        bradFinalMood = 0;

        angryCustomerListeningScore = 0;
        angryCustomerFinalMood = 0;

        angryCustomerName = "Customer 2";

        currentNPC = null;

        SetPanel(feedbackPanel, false);
        SetPanel(bradTrainingPanel, false);
        SetPanel(generalFeedbackPanel, false);
    }

    // Show feedback for the current customer
    public void ShowFeedback(
        NPCController npc,
        int finalMood,
        int listeningScore)
    {
        if (npc == null)
            return;

        if (gameManager == null)
        {
            Debug.LogError(
                "FeedbackManager: GameManager is not assigned."
            );

            return;
        }

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

        if (feedbackTitleText != null)
        {
            feedbackTitleText.text =
                "Interaction Feedback - " +
                npc.NPCName;
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
            angryCustomerListeningScore =
                listeningScore;

            angryCustomerFinalMood =
                finalMood;

            angryCustomerName =
                npc.NPCName;

            if (feedbackText != null)
            {
                feedbackText.text =
                    BuildAngryCustomerFeedback(
                        listeningScore,
                        finalMood
                    );
            }
        }

        SetPanel(feedbackPanel, true);
    }

    // Build Brad feedback
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
                " of " +
                listeningOpportunities +
                " opportunities.\n\n" +

                "You regularly reflected Brad's concerns " +
                "and gave him opportunities to explain further.";
        }
        else if (listeningScore >= 2)
        {
            listeningFeedback =
                "Active Listening: Developing\n\n" +

                "You demonstrated active listening in " +
                listeningScore +
                " of " +
                listeningOpportunities +
                " opportunities.\n\n" +

                "You used active listening at some points, " +
                "but missed opportunities to explore or " +
                "reflect Brad's concerns.";
        }
        else
        {
            listeningFeedback =
                "Active Listening: Limited\n\n" +

                "You demonstrated active listening in " +
                listeningScore +
                " of " +
                listeningOpportunities +
                " opportunities.\n\n" +

                "Several opportunities to reflect, clarify " +
                "or explore Brad's concerns were missed.";
        }

        return
            listeningFeedback +
            "\n\n" +
            BuildInteractionFeedback(finalMood);
    }

    // Build angry customer feedback
    private string BuildAngryCustomerFeedback(
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
                " of " +
                listeningOpportunities +
                " opportunities.\n\n" +

                "You consistently acknowledged the customer's " +
                "concerns and kept the interaction constructive.";
        }
        else if (listeningScore >= 2)
        {
            listeningFeedback =
                "Active Listening: Developing\n\n" +

                "You demonstrated active listening in " +
                listeningScore +
                " of " +
                listeningOpportunities +
                " opportunities.\n\n" +

                "You listened effectively at several points, " +
                "but some opportunities to reflect or clarify " +
                "the concern were missed.";
        }
        else
        {
            listeningFeedback =
                "Active Listening: Limited\n\n" +

                "You demonstrated active listening in " +
                listeningScore +
                " of " +
                listeningOpportunities +
                " opportunities.\n\n" +

                "Several opportunities to acknowledge, " +
                "clarify or explore the customer's concern " +
                "were missed.";
        }

        return
            listeningFeedback +
            "\n\n" +
            BuildInteractionFeedback(finalMood);
    }

    // Build interaction outcome feedback
    private string BuildInteractionFeedback(
        int finalMood)
    {
        if (finalMood > 0)
        {
            return
                "Interaction Outcome: Improving\n\n" +
                "Your choices helped move the interaction " +
                "in a more positive direction.";
        }

        if (finalMood == 0)
        {
            return
                "Interaction Outcome: Stable\n\n" +
                "The interaction did not clearly improve " +
                "or worsen overall.";
        }

        return
            "Interaction Outcome: Not Improving\n\n" +
            "The interaction remained difficult at the end.";
    }

    // Continue from customer feedback
    public void ContinueFromCustomerFeedback()
    {
        SetPanel(feedbackPanel, false);

        if (gameManager == null)
            return;

        if (gameManager.CurrentCustomerIndex == 0)
        {
            ShowBradTraining();
        }
        else
        {
            SendCurrentCustomerOut();
        }
    }

    // Show training after Brad
    private void ShowBradTraining()
    {
        if (bradTrainingText != null)
        {
            bradTrainingText.text =
                "Responding to Difficult Customers\n\n" +

                "LISTEN FIRST\n" +
                "Focus on what the customer is actually saying before you respond.\n\n" +

                "ACKNOWLEDGE THE EMOTION\n" +
                "Recognise how the customer feels without dismissing or judging them.\n\n" +

                "REFLECT AND PARAPHRASE\n" +
                "Restate the main concern in your own words to show understanding.\n\n" +

                "CLARIFY AND ASK\n" +
                "Ask relevant questions when you need more information.\n\n" +

                "RESPOND WITH EMPATHY\n" +
                "Consider the customer's perspective and respond supportively.\n\n" +

                "STAY CALM AND PROFESSIONAL\n" +
                "Avoid becoming defensive, dismissive or confrontational.\n\n" +

                "CHOOSE ACTIONS FROM THE CONTEXT\n" +
                "Base drinks and actions on what the customer has communicated.\n\n" +

                "MAINTAIN PROFESSIONAL BOUNDARIES\n" +
                "Be supportive while staying within your role and limits.\n\n" +

                "Apply these principles to the next customer.";
        }

        SetPanel(
            bradTrainingPanel,
            true
        );
    }

    // Continue to the angry customer
    public void ContinueAfterBradTraining()
    {
        SetPanel(
            bradTrainingPanel,
            false
        );

        SendCurrentCustomerOut();
    }

    // Send current customer out
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

    // Show overall feedback
    public void ShowGeneralFeedback()
    {
        string comparisonFeedback;

        if (angryCustomerListeningScore >
            bradListeningScore)
        {
            comparisonFeedback =
                "You demonstrated more active-listening " +
                "responses in the second interaction.";
        }
        else if (angryCustomerListeningScore ==
                 bradListeningScore)
        {
            comparisonFeedback =
                "Your active-listening score remained " +
                "the same across both interactions.";
        }
        else
        {
            comparisonFeedback =
                "You demonstrated fewer active-listening " +
                "responses in the second interaction.";
        }

        if (generalFeedbackText != null)
        {
            generalFeedbackText.text =
                "Overall Feedback\n\n" +

                "Brad\n" +
                "Active Listening: " +
                bradListeningScore +
                "/" +
                listeningOpportunities +
                "\n" +

                "Interaction Outcome: " +
                GetInteractionStatus(
                    bradFinalMood
                ) +
                "\n\n" +

                angryCustomerName +
                "\n" +

                "Active Listening: " +
                angryCustomerListeningScore +
                "/" +
                listeningOpportunities +
                "\n" +

                "Interaction Outcome: " +
                GetInteractionStatus(
                    angryCustomerFinalMood
                ) +
                "\n\n" +

                comparisonFeedback;
        }

        SetPanel(
            generalFeedbackPanel,
            true
        );
    }

    // Continue to post questionnaire
    public void CloseGeneralFeedback()
    {
        SetPanel(
            generalFeedbackPanel,
            false
        );

        if (gameManager != null)
            gameManager.GameplayCompleted();
    }

    // Convert mood value to interaction status
    private string GetInteractionStatus(
        int mood)
    {
        if (mood > 0)
            return "Improving";

        if (mood == 0)
            return "Stable";

        return "Not Improving";
    }

    // Change panel state
    private void SetPanel(
        GameObject panel,
        bool state)
    {
        if (panel != null)
            panel.SetActive(state);
    }
}