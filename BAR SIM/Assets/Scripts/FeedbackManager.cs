using UnityEngine;
using TMPro;

public class FeedbackManager : MonoBehaviour
{
    [Header("Individual Feedback UI")]
    [SerializeField]
    private GameObject feedbackPanel;

    [SerializeField]
    private TMP_Text feedbackText;

    [Header("General Feedback UI")]
    [SerializeField]
    private GameObject generalFeedbackPanel;

    [SerializeField]
    private TMP_Text generalFeedbackText;

    [Header("Systems")]
    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    private BartenderInteraction bartenderInteraction;

    [SerializeField]
    private GameManager gameManager;

    private NPCController currentNPC;

    private int completedInteractions;
    private int totalFinalMood;
    private int totalListeningScore;

    private void Awake()
    {
        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(false);
        }

        if (generalFeedbackPanel != null)
        {
            generalFeedbackPanel.SetActive(false);
        }
    }

    public void ResetFeedbackData()
    {
        completedInteractions = 0;
        totalFinalMood = 0;
        totalListeningScore = 0;

        currentNPC = null;

        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(false);
        }

        if (generalFeedbackPanel != null)
        {
            generalFeedbackPanel.SetActive(false);
        }
    }

    public void ShowFeedback(
        NPCController npc,
        int finalMood,
        int listeningScore)
    {
        currentNPC = npc;

        completedInteractions++;

        totalFinalMood += finalMood;

        totalListeningScore +=
            listeningScore;

        string moodFeedback;

        if (finalMood > 0)
        {
            moodFeedback =
                "The customer's emotional state improved during the interaction.";
        }
        else if (finalMood == 0)
        {
            moodFeedback =
                "The customer's emotional state remained relatively stable.";
        }
        else
        {
            moodFeedback =
                "The customer remained emotionally distressed at the end of the interaction.";
        }

        string listeningFeedback;

        if (listeningScore >= 2)
        {
            listeningFeedback =
                "You demonstrated strong active listening and acknowledgement.";
        }
        else if (listeningScore >= 1)
        {
            listeningFeedback =
                "You demonstrated some effective active listening behaviours.";
        }
        else
        {
            listeningFeedback =
                "There were opportunities to acknowledge and explore the customer's concerns more effectively.";
        }

        if (feedbackText != null)
        {
            feedbackText.text =
                moodFeedback +
                "\n\n" +
                listeningFeedback;
        }

        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(true);
        }
    }

    // Connect the individual feedback
    // Continue button to this method.
    public void ContinueFromCustomerFeedback()
    {
        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(false);
        }

        if (currentNPC == null)
            return;

        NPCController npcLeaving =
            currentNPC;

        currentNPC = null;

        // Return to Camera 1.
        if (cameraManager != null)
        {
            cameraManager.FollowNPC(
                npcLeaving
            );
        }

        if (bartenderInteraction != null)
        {
            bartenderInteraction
                .EndNPCInteraction();
        }

        // NPC walks to ExitPoint.
        npcLeaving.LeaveBar();
    }

    public void ShowGeneralFeedback()
    {
        if (generalFeedbackPanel == null)
            return;

        float averageMood = 0f;
        float averageListening = 0f;

        if (completedInteractions > 0)
        {
            averageMood =
                (float)totalFinalMood /
                completedInteractions;

            averageListening =
                (float)totalListeningScore /
                completedInteractions;
        }

        string overallMood;

        if (averageMood > 0f)
        {
            overallMood =
                "Across both interactions, you generally improved the customers' emotional states.";
        }
        else if (averageMood == 0f)
        {
            overallMood =
                "Across both interactions, the customers' emotional states remained relatively stable.";
        }
        else
        {
            overallMood =
                "Across both interactions, the customers remained relatively distressed.";
        }

        string overallListening;

        if (averageListening >= 2f)
        {
            overallListening =
                "Your overall active-listening performance was strong.";
        }
        else if (averageListening >= 1f)
        {
            overallListening =
                "You demonstrated several appropriate active-listening behaviours.";
        }
        else
        {
            overallListening =
                "Your interactions showed opportunities for stronger active listening and acknowledgement.";
        }

        if (generalFeedbackText != null)
        {
            generalFeedbackText.text =
                "Overall Interaction Feedback\n\n" +
                overallMood +
                "\n\n" +
                overallListening;
        }

        generalFeedbackPanel.SetActive(true);
    }

    // Connects the General Feedback
    // Continue button to this method.
    public void CloseGeneralFeedback()
    {
        if (generalFeedbackPanel != null)
        {
            generalFeedbackPanel.SetActive(false);
        }

        if (gameManager != null)
        {
            gameManager.ShowGameOver();
        }
    }
}