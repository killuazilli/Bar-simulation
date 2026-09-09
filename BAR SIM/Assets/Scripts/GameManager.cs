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
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject participantIDPanel;
    [SerializeField] private GameObject preQuestionnairePanel;
    [SerializeField] private GameObject trainingIntroPanel;
    [SerializeField] private GameObject postQuestionnairePanel;

    private int currentCustomerIndex = -1;

    public int CurrentCustomerIndex =>
        currentCustomerIndex;

    private void Awake()
    {
        if (customers != null)
        {
            foreach (NPCController customer in customers)
            {
                if (customer != null)
                    customer.gameObject.SetActive(false);
            }
        }

        SetPanel(mainMenuPanel, true);
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

    // Open participant ID entry
    public void PlayPressed()
    {
        SetPanel(mainMenuPanel, false);
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

    // Start Brad's interaction
    public void BeginBradInteraction()
    {
        SetPanel(trainingIntroPanel, false);

        currentCustomerIndex = -1;

        if (feedbackManager != null)
            feedbackManager.ResetFeedbackData();

        StartNextCustomer();
    }

    // Start the next customer
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

    // Continue after NPC leaves
    public void CustomerExited(
        NPCController customer)
    {
        if (customer != null)
            customer.gameObject.SetActive(false);

        bool anotherCustomerExists =
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

    // Show overall feedback
    private void ShowFinalFeedback()
    {
        if (cameraManager != null)
            cameraManager.StopFollowing();

        if (feedbackManager != null)
            feedbackManager.ShowGeneralFeedback();
    }

    // Finish gameplay and show post-questionnaire
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

    // Finish study and return to menu
    public void FinishStudyAndReturnToMainMenu()
    {
        if (researchDataManager != null)
            researchDataManager.SaveGameplayData();

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
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