using UnityEngine;
using TMPro;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] private MoodSystem moodSystem;
    [SerializeField] private GameObject FeedbackPanel;
    [SerializeField] private TMP_Text FeedbackText;

    public void CalculateOutcome()
    {
        int mood = moodSystem.CurrentMood;

        FeedbackPanel.SetActive(true);

        if (mood >= 2)
        {
            FeedbackText.text =
                "Positive Outcome\n\nThe customer felt heard and supported.";
        }
        else if (mood >= 0)
        {
            FeedbackText.text =
                "Neutral Outcome\n\nThe interaction was resolved, but the customer remained emotionally unsettled.";
        }
        else
        {
            FeedbackText.text =
                "Negative Outcome\n\nThe customer's emotional state did not improve.";
        }
    }
}