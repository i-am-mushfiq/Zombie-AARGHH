using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputName;

    [SerializeField]
    private Leaderboard leaderboard;

    private bool hasSubmitted = false;

    public void SubmitScore()
    {
        if (hasSubmitted)
        {
            Debug.LogWarning("Score has already been submitted for this game instance.");
            return;
        }

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

        if (leaderboard != null)
        {
            leaderboard.AddLeaderboardEntry(username, score);
            Debug.Log("Leaderboard updated with new entry.");
            hasSubmitted = true;
        }
        else
        {
            Debug.LogError("Leaderboard is not assigned in the LeaderboardManager.");
        }
    }
}
