using UnityEngine;

public class QuestionnaireLinkManager : MonoBehaviour
{
    [Header("Microsoft Forms")]
    [SerializeField] private string preQuestionnaireURL;
    [SerializeField] private string postQuestionnaireURL;

    // Open the pre-questionnaire
    public void OpenPreQuestionnaire()
    {
        if (string.IsNullOrEmpty(preQuestionnaireURL))
        {
            Debug.LogError(
                "Pre-questionnaire URL is empty."
            );

            return;
        }

        Application.OpenURL(
            preQuestionnaireURL
        );
    }

    // Open the post-questionnaire
    public void OpenPostQuestionnaire()
    {
        if (string.IsNullOrEmpty(postQuestionnaireURL))
        {
            Debug.LogError(
                "Post-questionnaire URL is empty."
            );

            return;
        }

        Application.OpenURL(
            postQuestionnaireURL
        );
    }
}