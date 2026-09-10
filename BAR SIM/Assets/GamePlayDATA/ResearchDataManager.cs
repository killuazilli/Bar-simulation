using UnityEngine;
using System;
using System.IO;
using System.Text;

public class ResearchDataManager : MonoBehaviour
{
    [Header("Computer")]
    [SerializeField] private ComputerIDManager computerIDManager;

    private ParticipantGameplayData currentData;
    private bool dataSaved;

    public ParticipantGameplayData CurrentData =>
        currentData;

    // Start a new participant session
    public void StartSession(string participantID)
    {
        currentData =
            new ParticipantGameplayData();

        if (computerIDManager != null)
        {
            currentData.computerID =
                computerIDManager.ComputerID;
        }

        currentData.participantID =
            participantID;

        currentData.sessionDateTime =
            DateTime.Now.ToString(
                "yyyy-MM-dd HH:mm:ss"
            );

        currentData.bradName = "";
        currentData.bradDrinkChoice = "";
        currentData.bradDialogueChoices = "";

        currentData.customer2Name = "";
        currentData.customer2DrinkChoice = "";
        currentData.customer2DialogueChoices = "";

        dataSaved = false;

        Debug.Log(
            "Research session started | Computer: " +
            currentData.computerID +
            " | Participant: " +
            participantID
        );
    }

    // Record dialogue choice
    public void RecordDialogueChoice(
        int customerIndex,
        string choiceText)
    {
        if (currentData == null)
            return;

        if (customerIndex == 0)
        {
            currentData.bradDialogueChoices =
                AddChoice(
                    currentData.bradDialogueChoices,
                    choiceText
                );
        }
        else if (customerIndex == 1)
        {
            currentData.customer2DialogueChoices =
                AddChoice(
                    currentData.customer2DialogueChoices,
                    choiceText
                );
        }
    }

    // Record drink or service choice
    public void RecordDrinkChoice(
        int customerIndex,
        string choiceID)
    {
        if (currentData == null)
            return;

        if (customerIndex == 0)
        {
            currentData.bradDrinkChoice =
                choiceID;
        }
        else if (customerIndex == 1)
        {
            currentData.customer2DrinkChoice =
                choiceID;
        }
    }

    // Record final customer result
    public void RecordCustomerResult(
        int customerIndex,
        string npcName,
        int listeningScore,
        int finalMood)
    {
        if (currentData == null)
            return;

        if (customerIndex == 0)
        {
            currentData.bradName =
                npcName;

            currentData.bradListeningScore =
                listeningScore;

            currentData.bradFinalMood =
                finalMood;
        }
        else if (customerIndex == 1)
        {
            currentData.customer2Name =
                npcName;

            currentData.customer2ListeningScore =
                listeningScore;

            currentData.customer2FinalMood =
                finalMood;
        }
    }

    // Save gameplay data to CSV
    public void SaveGameplayData()
    {
        if (currentData == null)
        {
            Debug.LogError(
                "No participant data exists to save."
            );

            return;
        }

        if (dataSaved)
            return;

        string filePath =
            Path.Combine(
                Application.persistentDataPath,
                "BehindTheBarGameplayData.csv"
            );

        bool fileExists =
            File.Exists(filePath);

        StringBuilder builder =
            new StringBuilder();

        if (!fileExists)
        {
            builder.AppendLine(
                "ComputerID," +
                "ParticipantID," +
                "DateTime," +
                "BradName," +
                "BradListeningScore," +
                "BradFinalMood," +
                "BradDrinkChoice," +
                "BradDialogueChoices," +
                "Customer2Name," +
                "Customer2ListeningScore," +
                "Customer2FinalMood," +
                "Customer2DrinkChoice," +
                "Customer2DialogueChoices"
            );
        }

        builder.AppendLine(
            CSV(currentData.computerID) + "," +
            CSV(currentData.participantID) + "," +
            CSV(currentData.sessionDateTime) + "," +
            CSV(currentData.bradName) + "," +
            currentData.bradListeningScore + "," +
            currentData.bradFinalMood + "," +
            CSV(currentData.bradDrinkChoice) + "," +
            CSV(currentData.bradDialogueChoices) + "," +
            CSV(currentData.customer2Name) + "," +
            currentData.customer2ListeningScore + "," +
            currentData.customer2FinalMood + "," +
            CSV(currentData.customer2DrinkChoice) + "," +
            CSV(currentData.customer2DialogueChoices)
        );

        File.AppendAllText(
            filePath,
            builder.ToString()
        );

        dataSaved = true;

        Debug.Log(
            "Gameplay data saved to: " +
            filePath
        );
    }

    // Add dialogue choice to history
    private string AddChoice(
        string existingChoices,
        string newChoice)
    {
        if (string.IsNullOrEmpty(existingChoices))
            return newChoice;

        return existingChoices +
               " | " +
               newChoice;
    }

    // Make text safe for CSV
    private string CSV(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "\"\"";

        value =
            value.Replace("\"", "\"\"")
                 .Replace("\n", " ")
                 .Replace("\r", " ");

        return "\"" + value + "\"";
    }
}