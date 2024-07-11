using Firebase.Auth;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;

public class LocalDataManager : MonoBehaviour
{
    public static LocalDataManager Instance { get; private set; }
    public GameData gameData;
    private string filePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        string hashedFileName = GetHashedFileName(userId);
        filePath = Path.Combine(Application.persistentDataPath, hashedFileName + ".json");
        LoadGameData();
    }

    private string GetHashedFileName(string input)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    private void LoadGameData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            gameData = JsonConvert.DeserializeObject<GameData>(jsonData);
        }
        else
        {
            gameData = new GameData();
            SaveGameData();
        }
    }

    public void SaveGameData()
    {
        string jsonData = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        File.WriteAllText(filePath, jsonData);
    }

    public void AddGameSession(string game, string date, int lvl, int star, int try_count, int corr,
        int ans_rate, int time, int conc)
    {
        GameSession newSession = new(date, lvl, star, try_count, corr,
                                    ans_rate, time, conc);

        switch (game)
        {
            case "vm":
                gameData.vm.Add(newSession);
                break;
            case "sp":
                gameData.sp.Add(newSession);
                break;
            case "fg":
                gameData.fg.Add(newSession);
                break;
            case "pc":
                gameData.pc.Add(newSession);
                break;
            case "sr":
                gameData.sr.Add(newSession);
                break;
            default:
                Debug.LogError($"Game {game} not recognized.");
                break;
        }

        SaveGameData();
    }

    public List<GameSession> GetGameSessions(string game)
    {
        switch (game)
        {
            case "vm":
                return gameData.vm;
            case "sp":
                return gameData.sp;
            case "fg":
                return gameData.fg;
            case "pc":
                return gameData.pc;
            case "sr":
                return gameData.sr;
            default:
                Debug.LogError($"Game {game} not recognized.");
                return new List<GameSession>();
        }
    }
}
