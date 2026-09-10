using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class ParticipantSessionManager : MonoBehaviour
{
    [Header("Participant Input")]
    [SerializeField] private TMP_InputField participantIDInput;
    [SerializeField] private TMP_Text errorText;

    [Header("Participant HUD")]
    [SerializeField] private GameObject participantHUD;
    [SerializeField] private TMP_Text participantHUDText;

    [Header("Systems")]
    [SerializeField] private ResearchDataManager researchDataManager;
    [SerializeField] private GameManager gameManager;

    private string currentParticipantID = "";

    public string CurrentParticipantID =>
        currentParticipantID;

    private void Start()
    {
        if (participantHUD != null)
            participantHUD.SetActive(false);

        if (participantIDInput != null)
            participantIDInput.text = "";

        if (errorText != null)
            errorText.text = "";
    }

    // Submit the assigned participant ID
    public void SubmitParticipantID()
    {
        if (participantIDInput == null)
            return;

        string enteredID =
            participantIDInput.text.Trim();

        if (string.IsNullOrEmpty(enteredID))
        {
            ShowError(
                "Please enter your Participant ID."
            );

            return;
        }

        if (!Regex.IsMatch(
            enteredID,
            @"^[A-Za-z0-9_-]+$"))
        {
            ShowError(
                "Please use only letters, numbers, - or _."
            );

            return;
        }

        currentParticipantID =
            enteredID.ToUpperInvariant();

        if (participantHUDText != null)
        {
            participantHUDText.text =
                "Participant: " +
                currentParticipantID;
        }

        if (participantHUD != null)
            participantHUD.SetActive(true);

        if (researchDataManager != null)
        {
            researchDataManager.StartSession(
                currentParticipantID
            );
        }

        if (errorText != null)
            errorText.text = "";

        if (gameManager != null)
            gameManager.ParticipantIDAccepted();
    }

    // Show input error
    private void ShowError(string message)
    {
        if (errorText != null)
            errorText.text = message;
    }
}