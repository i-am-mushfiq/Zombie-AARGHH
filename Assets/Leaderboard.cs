using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    private string publicLeaderboardKey = "f20d02ac4098b765eef4e0a9805f8127ab7a9824cd891cc5ae7d8c548afcaa91";

    public void GetLeaderboard()
    {
        Debug.Log("Requesting leaderboard data...");

        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (msg) =>
        {
            if (msg == null || msg.Length == 0)
            {
                Debug.LogError("Leaderboard data is null or empty.");
                return;
            }

            Debug.Log("Leaderboard data received. Updating UI...");

            for (int i = 0; i < names.Count; ++i)
            {
                if (i < msg.Length)
                {
                    Debug.Log($"Updating entry {i}: Username = {msg[i].Username}, Score = {msg[i].Score}");
                    names[i].text = msg[i].Username;
                    scores[i].text = msg[i].Score.ToString();
                }
                else
                {
                    Debug.LogWarning($"Leaderboard data does not contain enough entries for index {i}. Updating with default values.");
                    names[i].text = "N/A";
                    scores[i].text = "0";
                }
            }
        });
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        Debug.Log($"Setting new leaderboard entry: Username = {username}, Score = {score}");

        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, (msg) =>
        {
            Debug.Log("Leaderboard entry uploaded successfully.");
            GetLeaderboard();
        });
    }
}
