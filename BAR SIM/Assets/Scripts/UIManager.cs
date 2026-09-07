using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject drinksPanel;
    [SerializeField] private GameObject moodPanel;
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private GameObject generalFeedbackPanel;
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        HideAllUI();
    }

    public void HideAllUI()
    {
        SetPanelActive(dialoguePanel, false);
        SetPanelActive(drinksPanel, false);
        SetPanelActive(moodPanel, false);
        SetPanelActive(feedbackPanel, false);
        SetPanelActive(generalFeedbackPanel, false);
        SetPanelActive(interactionPrompt, false);
        SetPanelActive(gameOverPanel, false);
    }

    private void SetPanelActive(GameObject panel, bool active)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }
    }
}