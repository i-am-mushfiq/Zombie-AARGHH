using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    private string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "leaderboard.dat");
        LoadLeaderboard();
    }

    public void SaveLeaderboard()
    {
        List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();

        for (int i = 0; i < names.Count; ++i)
        {
            leaderboardEntries.Add(new LeaderboardEntry(names[i].text, int.Parse(scores[i].text)));
        }

        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(stream, leaderboardEntries);
        }
        Debug.Log("Leaderboard saved to " + filePath);
    }

    public void LoadLeaderboard()
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                List<LeaderboardEntry> leaderboardEntries = formatter.Deserialize(stream) as List<LeaderboardEntry>;

                if (leaderboardEntries != null)
                {
                    for (int i = 0; i < names.Count; ++i)
                    {
                        if (i < leaderboardEntries.Count)
                        {
                            names[i].text = leaderboardEntries[i].Username;
                            scores[i].text = leaderboardEntries[i].Score.ToString();
                        }
                        else
                        {
                            names[i].text = "N/A";
                            scores[i].text = "0";
                        }
                    }
                }
                else
                {
                    Debug.LogError("Failed to deserialize leaderboard data.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Leaderboard file not found. Initializing with default values.");
            InitializeDefaultLeaderboard();
        }
    }

    public void AddLeaderboardEntry(string username, int score)
    {
        Debug.Log($"Adding new leaderboard entry: Username = {username}, Score = {score}");
        InsertEntry(username, score);
        SaveLeaderboard();
    }

    private void InsertEntry(string username, int score)
    {
        // Find the correct position to insert the new entry
        int insertIndex = -1;
        for (int i = 0; i < names.Count; ++i)
        {
            if (names[i].text == "N/A" || int.Parse(scores[i].text) < score)
            {
                insertIndex = i;
                break;
            }
        }

        // If there's space for a new entry or the new score is higher than some existing scores
        if (insertIndex >= 0)
        {
            // Shift entries down to make room for the new entry
            for (int i = names.Count - 1; i > insertIndex; --i)
            {
                names[i].text = names[i - 1].text;
                scores[i].text = scores[i - 1].text;
            }

            // Insert the new entry
            names[insertIndex].text = username;
            scores[insertIndex].text = score.ToString();
        }
    }

    private void InitializeDefaultLeaderboard()
    {
        for (int i = 0; i < names.Count; ++i)
        {
            names[i].text = "N/A";
            scores[i].text = "0";
        }
    }
}
