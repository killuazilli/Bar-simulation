using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Customers")]
    [SerializeField]
    private NPCController[] customers;

    [Header("Systems")]
    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    private FeedbackManager feedbackManager;

    [Header("Game Over")]
    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private string mainMenuSceneName =
        "MainMenu";

    private int currentCustomerIndex = -1;

    private void Awake()
    {
        if (customers != null)
        {
            foreach (
                NPCController customer
                in customers)
            {
                if (customer != null)
                {
                    customer.gameObject
                        .SetActive(false);
                }
            }
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        currentCustomerIndex = -1;

        if (feedbackManager != null)
        {
            feedbackManager
                .ResetFeedbackData();
        }

        StartNextCustomer();
    }

    private void StartNextCustomer()
    {
        currentCustomerIndex++;

        if (currentCustomerIndex >=
            customers.Length)
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

        // Camera 1 follows the NPC.
        if (cameraManager != null)
        {
            cameraManager.FollowNPC(
                customer
            );
        }

        // NPC begins walking toward
        // BarServicePoint.
        customer.BeginVisit();
    }

    // Called by NPCController after
    // reaching ExitPoint.
    public void CustomerExited(
        NPCController customer)
    {
        if (customer != null)
        {
            customer.gameObject
                .SetActive(false);
        }

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

    private void ShowFinalFeedback()
    {
        if (cameraManager != null)
        {
            cameraManager.StopFollowing();
        }

        if (feedbackManager != null)
        {
            feedbackManager
                .ShowGeneralFeedback();
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // Connect Retry button.
    public void RetryGame()
    {
        SceneManager.LoadScene(
            SceneManager
                .GetActiveScene()
                .buildIndex
        );
    }

    // Connect Main Menu button.
    public void BackToMainMenu()
    {
        if (string.IsNullOrEmpty(
            mainMenuSceneName))
        {
            Debug.LogError(
                "Main Menu scene name has not been assigned."
            );

            return;
        }

        SceneManager.LoadScene(
            mainMenuSceneName
        );
    }
}