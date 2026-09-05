using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoodSystem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text moodText;
    [SerializeField] private Image moodIcon;

    private int currentMood;

    public int CurrentMood => currentMood;

    public void SetMood(int value)
    {
        currentMood =
            Mathf.Clamp(value, -2, 2);

        UpdateMoodUI();
    }

    private void UpdateMoodUI()
    {
        if (currentMood <= -2)
        {
            moodText.text = "Very Negative";
        }
        else if (currentMood == -1)
        {
            moodText.text = "Negative";
        }
        else if (currentMood == 0)
        {
            moodText.text = "Neutral";
        }
        else if (currentMood == 1)
        {
            moodText.text = "Improving";
        }
        else
        {
            moodText.text = "Positive";
        }
    }
}