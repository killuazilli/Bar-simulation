using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Customers")]
    [SerializeField] private NPCController[] customers;

    [Header("Systems")]
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private FeedbackManager feedbackManager;
    [SerializeField] private MoodSystem moodSystem;
    [SerializeField] private ResearchDataManager researchDataManager;

    [Header("Flow UI")]
    [SerializeField] private GameObject participantIDPanel;
    [SerializeField] private GameObject preQuestionnairePanel;
    [SerializeField] private GameObject trainingIntroPanel;
    [SerializeField] private GameObject postQuestionnairePanel;

    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private int currentCustomerIndex = -1;

    public int CurrentCustomerIndex => currentCustomerIndex;

    private void Awake()
    {
        // Hide all customers
        if (customers != null)
        {
            foreach (NPCController customer in customers)
            {
                if (customer != null)
                    customer.gameObject.SetActive(false);
            }
        }

        // ComputerIDManager decides when Participant ID appears
        SetPanel(participantIDPanel, false);
        SetPanel(preQuestionnairePanel, false);
        SetPanel(trainingIntroPanel, false);
        SetPanel(postQuestionnairePanel, false);
    }

    private void Start()
    {
        currentCustomerIndex = -1;

        if (feedbackManager != null)
            feedbackManager.ResetFeedbackData();

        if (moodSystem != null)
            moodSystem.HideMood();

        if (cameraManager != null)
            cameraManager.StopFollowing();
    }

    // Show participant ID screen
    public void ShowParticipantIDPanel()
    {
        SetPanel(participantIDPanel, true);
    }

    // Continue after participant ID
    public void ParticipantIDAccepted()
    {
        SetPanel(participantIDPanel, false);
        SetPanel(preQuestionnairePanel, true);
    }

    // Continue after pre-questionnaire
    public void PreQuestionnaireCompleted()
    {
        SetPanel(preQuestionnairePanel, false);
        SetPanel(trainingIntroPanel, true);
    }

    // Start Brad interaction
    public void BeginBradInteraction()
    {
        SetPanel(trainingIntroPanel, false);

        currentCustomerIndex = -1;

        if (feedbackManager != null)
            feedbackManager.ResetFeedbackData();

        StartNextCustomer();
    }

    // Start next customer
    private void StartNextCustomer()
    {
        currentCustomerIndex++;

        if (customers == null ||
            currentCustomerIndex >= customers.Length)
        {
            ShowFinalFeedback();
            return;
        }

        NPCController customer =
            customers[currentCustomerIndex];

        if (customer == null)
        {
            Debug.LogError(
                "Customer slot " +
                currentCustomerIndex +
                " is empty."
            );

            return;
        }

        customer.gameObject.SetActive(true);

        if (moodSystem != null &&
            customer.NPCData != null)
        {
            moodSystem.SetNPCData(
                customer.NPCData
            );
        }

        if (cameraManager != null)
        {
            cameraManager.FollowNPC(
                customer
            );
        }

        customer.BeginVisit();
    }

    // Called when a customer leaves
    public void CustomerExited(
        NPCController customer)
    {
        if (customer != null)
            customer.gameObject.SetActive(false);

        bool anotherCustomerExists =
            customers != null &&
            currentCustomerIndex <
            customers.Length - 1;

        if (anotherCustomerExists)
        {
            StartNextCustomer();
        }
        else
        {
            ShowFinalFeedback();
        }
    }

    // Show final feedback
    private void ShowFinalFeedback()
    {
        if (cameraManager != null)
            cameraManager.StopFollowing();

        if (feedbackManager != null)
            feedbackManager.ShowGeneralFeedback();
    }

    // Save data and show post questionnaire
    public void GameplayCompleted()
    {
        if (researchDataManager != null)
            researchDataManager.SaveGameplayData();

        if (moodSystem != null)
            moodSystem.HideMood();

        SetPanel(
            postQuestionnairePanel,
            true
        );
    }

    // Return to Main Menu
    public void FinishStudyAndReturnToMainMenu()
    {
        if (researchDataManager != null)
            researchDataManager.SaveGameplayData();

        SceneManager.LoadScene(
            mainMenuSceneName
        );
    }

    // Change panel visibility
    private void SetPanel(
        GameObject panel,
        bool state)
    {
        if (panel != null)
            panel.SetActive(state);
    }
}