using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoodSystem : MonoBehaviour
{
    [Header("Mood")]
    [SerializeField] private int currentMood = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text moodText;
    [SerializeField] private Image moodIcon;

    [Header("Icons")]
    [SerializeField] private Sprite veryNegativeIcon;
    [SerializeField] private Sprite negativeIcon;
    [SerializeField] private Sprite neutralIcon;
    [SerializeField] private Sprite positiveIcon;
    [SerializeField] private Sprite veryPositiveIcon;

    public int CurrentMood => currentMood;

    public void SetMood(int value)
    {
        currentMood = Mathf.Clamp(value, -2, 2);
        UpdateMoodUI();
    }

    public void ChangeMood(int amount)
    {
        currentMood += amount;
        currentMood = Mathf.Clamp(currentMood, -2, 2);

        UpdateMoodUI();
    }

    private void UpdateMoodUI()
    {
        switch (currentMood)
        {
            case -2:
                moodText.text = "Very Negative";
                moodIcon.sprite = veryNegativeIcon;
                break;

            case -1:
                moodText.text = "Negative";
                moodIcon.sprite = negativeIcon;
                break;

            case 0:
                moodText.text = "Neutral";
                moodIcon.sprite = neutralIcon;
                break;

            case 1:
                moodText.text = "Positive";
                moodIcon.sprite = positiveIcon;
                break;

            case 2:
                moodText.text = "Very Positive";
                moodIcon.sprite = veryPositiveIcon;
                break;
        }
    }
}