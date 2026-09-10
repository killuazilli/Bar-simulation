using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Gameplay UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject drinksPanel;
    [SerializeField] private GameObject moodPanel;
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private GameObject generalFeedbackPanel;
    [SerializeField] private GameObject interactionPrompt;

    private void Awake()
    {
        SetPanel(dialoguePanel, false);
        SetPanel(drinksPanel, false);
        SetPanel(moodPanel, false);
        SetPanel(feedbackPanel, false);
        SetPanel(generalFeedbackPanel, false);
        SetPanel(interactionPrompt, false);
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