using UnityEngine;
using TMPro;

public class ComputerIDManager : MonoBehaviour
{
    [Header("Computer Setup UI")]
    [SerializeField] private GameObject setupPanel;
    [SerializeField] private TMP_InputField computerIDInput;
    [SerializeField] private TMP_Text currentComputerIDText;

    private const string ComputerIDKey = "ComputerID";

    public string ComputerID =>
        PlayerPrefs.GetString(
            ComputerIDKey,
            ""
        );

    private void Start()
    {
        CheckComputerID();
    }

    // Check if this computer has already been configured
    private void CheckComputerID()
    {
        if (string.IsNullOrEmpty(ComputerID))
        {
            if (setupPanel != null)
                setupPanel.SetActive(true);

            return;
        }

        if (setupPanel != null)
            setupPanel.SetActive(false);

        UpdateDisplay();
    }

    // Save this computer's ID
    public void SaveComputerID()
    {
        if (computerIDInput == null)
            return;

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

        Debug.Log(
            "Computer configured as: " +
            newID
        );
    }

    // Update the computer ID display
    private void UpdateDisplay()
    {
        if (currentComputerIDText != null)
        {
            currentComputerIDText.text =
                "Computer: " +
                ComputerID;
        }
    }
}