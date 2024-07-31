using System.Collections;
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
            // Initialize with default values if the file is not found
            for (int i = 0; i < names.Count; ++i)
            {
                names[i].text = "N/A";
                scores[i].text = "0";
            }
        }
    }

    public void AddLeaderboardEntry(string username, int score)
    {
        Debug.Log($"Adding new leaderboard entry: Username = {username}, Score = {score}");

        // Find if the username already exists and update the score
        bool entryUpdated = false;
        for (int i = 0; i < names.Count; ++i)
        {
            if (names[i].text == username)
            {
                scores[i].text = score.ToString();
                entryUpdated = true;
                break;
            }
        }

        // If the username doesn't exist, add a new entry
        if (!entryUpdated)
        {
            for (int i = 0; i < names.Count; ++i)
            {
                if (names[i].text == "N/A")
                {
                    names[i].text = username;
                    scores[i].text = score.ToString();
                    break;
                }
            }
        }

        SaveLeaderboard();
    }
}
