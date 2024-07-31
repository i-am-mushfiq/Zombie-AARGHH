using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputName;

    public UnityEvent<string, int> SubmitScoreEvent;

    public void SubmitScore()
    {
        string username = inputName.text;
        int score = ScoreManager.Instance.GetScore();

        Debug.Log($"SubmitScore called. Username: {username}, Score: {score}");

        if (string.IsNullOrWhiteSpace(username))
        {
            Debug.LogWarning("Username is empty or whitespace. Please enter a valid username.");
            return;
        }

        if (score < 0)
        {
            Debug.LogWarning("Score is negative. Please ensure the score is a valid non-negative number.");
            return;
        }

        Debug.Log("Invoking SubmitScoreEvent...");
        SubmitScoreEvent.Invoke(username, score);
        Debug.Log("SubmitScoreEvent invoked successfully.");
    }
}
