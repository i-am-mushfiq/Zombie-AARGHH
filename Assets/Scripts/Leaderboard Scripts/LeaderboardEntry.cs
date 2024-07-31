using System;
[Serializable]
public class LeaderboardEntry
{
    public string Username;
    public int Score;

    public LeaderboardEntry(string username, int score)
    {
        Username = username;
        Score = score;
    }
}
