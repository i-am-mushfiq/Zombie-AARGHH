using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI killStreakText; // Optional: Display kill streak in the UI

    private int score = 0;
    private int killStreak = 0; // Counter for the kill streak

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoints(int points)
    {
        score += points;
        killStreak++;
        UpdateScoreText();
        UpdateKillStreakText();
    }

    public void ResetKillStreak()
    {
        killStreak = 0;
        UpdateKillStreakText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateKillStreakText()
    {
        if (killStreakText != null)
        {
            killStreakText.text = "Kill Streak: " + killStreak;
        }
    }

    public int GetScore()
    {
        return score;
    }

    public int GetKillStreak()
    {
        return killStreak;
    }
}
