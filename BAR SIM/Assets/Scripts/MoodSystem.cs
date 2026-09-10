using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoodSystem : MonoBehaviour
{
    [Header("Mood UI")]
    [SerializeField] private GameObject moodPanel;
    [SerializeField] private TMP_Text moodText;
    [SerializeField] private Image moodIcon;

    private int currentMood;
    private NPCData currentNPCData;

    public int CurrentMood =>
        currentMood;

    private void Start()
    {
        HideMood();
    }

    // Set the active NPC
    public void SetNPCData(NPCData npcData)
    {
        if (npcData == null)
            return;

        currentNPCData =
            npcData;

        SetMood(
            currentNPCData.startingMood
        );
    }

    // Update mood value
    public void SetMood(int value)
    {
        currentMood =
            Mathf.Clamp(value, -2, 2);

        ShowMood();
        UpdateMoodUI();
    }

    // Update mood text and icon
    private void UpdateMoodUI()
    {
        if (moodText != null)
        {
            if (currentMood <= -2)
                moodText.text = "Very Negative";
            else if (currentMood == -1)
                moodText.text = "Negative";
            else if (currentMood == 0)
                moodText.text = "Neutral";
            else if (currentMood == 1)
                moodText.text = "Improving";
            else
                moodText.text = "Positive";
        }

        if (moodIcon != null &&
            currentNPCData != null)
        {
            if (currentMood < 0)
            {
                moodIcon.sprite =
                    currentNPCData.negativeIcon;
            }
            else if (currentMood > 0)
            {
                moodIcon.sprite =
                    currentNPCData.positiveIcon;
            }
            else
            {
                moodIcon.sprite =
                    currentNPCData.neutralIcon;
            }

            moodIcon.enabled =
                moodIcon.sprite != null;
        }
    }

    // Show mood panel
    public void ShowMood()
    {
        if (moodPanel != null)
            moodPanel.SetActive(true);
    }

    // Hide mood panel
    public void HideMood()
    {
        if (moodPanel != null)
            moodPanel.SetActive(false);
    }
}