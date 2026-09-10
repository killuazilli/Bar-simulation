using UnityEngine;
using TMPro;

public class ComputerIDManager : MonoBehaviour
{
    [Header("Computer Setup UI")]
    [SerializeField] private GameObject setupPanel;
    [SerializeField] private TMP_InputField computerIDInput;
    [SerializeField] private TMP_Text currentComputerIDText;

    [Header("Systems")]
    [SerializeField] private GameManager gameManager;

    private const string ComputerIDKey =
        "ComputerID";

    public string ComputerID =>
        PlayerPrefs.GetString(
            ComputerIDKey,
            ""
        );

    private void Start()
    {
        CheckComputerID();
    }

    // Check if computer already has an ID
    private void CheckComputerID()
    {
        if (string.IsNullOrEmpty(ComputerID))
        {
            // First time on this computer
            if (setupPanel != null)
                setupPanel.SetActive(true);

            Debug.Log(
                "No Computer ID found."
            );

            return;
        }

        // Computer already configured
        if (setupPanel != null)
            setupPanel.SetActive(false);

        UpdateDisplay();

        if (gameManager != null)
        {
            gameManager
                .ShowParticipantIDPanel();
        }

        Debug.Log(
            "Computer ID found: " +
            ComputerID
        );
    }

    // Save computer ID
    public void SaveComputerID()
    {
        if (computerIDInput == null)
        {
            Debug.LogError(
                "Computer ID Input is not assigned."
            );

            return;
        }

        string newID =
            computerIDInput.text
                .Trim()
                .ToUpperInvariant();

        if (string.IsNullOrEmpty(newID))
        {
            Debug.LogWarning(
                "Computer ID cannot be empty."
            );

            return;
        }

        PlayerPrefs.SetString(
            ComputerIDKey,
            newID
        );

        PlayerPrefs.Save();

        if (setupPanel != null)
            setupPanel.SetActive(false);

        UpdateDisplay();

        if (gameManager != null)
        {
            gameManager
                .ShowParticipantIDPanel();
        }

        Debug.Log(
            "Computer configured as: " +
            newID
        );
    }

    // Update Computer ID display
    private void UpdateDisplay()
    {
        if (currentComputerIDText != null)
        {
            currentComputerIDText.text =
                "Computer: " +
                ComputerID;
        }
    }

    // Reset saved computer ID
    public void ResetComputerID()
    {
        PlayerPrefs.DeleteKey("ComputerID");
        PlayerPrefs.Save();

        Debug.Log("Computer ID reset.");
    }
}