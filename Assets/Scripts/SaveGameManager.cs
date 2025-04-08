using System.IO;
using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    [System.Serializable]
    public class GameData
    {
        public int numberOfProblems;
        public int curProblem;
        public float timePerProblem;
        public float remainingTime;
        public int difficulty;
        public string playerName;
        public float spawnRate;
        public int score;
        public bool isWin;
        public PlayerController playerController;
    }
    //public static SaveGameManager instance; // Design pattern: Singleton
    public void SaveGame(/*GameManager gameManager*/)
    {
        Debug.Log("Get into SaveGame");
        Debug.Log(PlayerInfo.score);
        Debug.Log(GameManager.instance.curProblem);
        GameData gameData = new GameData()
        {
            difficulty = PlayerInfo.difficulty,
            spawnRate = PlayerInfo.spawnRate,
            timePerProblem = PlayerInfo.timePerProblem,
            numberOfProblems = PlayerInfo.numberOfProblems,

            score = PlayerInfo.score,
            curProblem = GameManager.instance.curProblem,
            remainingTime = GameManager.instance.remainingTime,
            isWin = PlayerInfo.isWin,
            playerController = GameManager.instance.player,
            //difficulty = gameManager.difficulty,
            //spawnRate = PlayerInfo.spawnRate,
            //numberOfProblems = gameManager.numberOfProblems,
            //curProblem = gameManager.curProblem,
            //timePerProblem = gameManager.timePerProblem,
            //remainingTime = gameManager.remainingTime,
            //playerController = gameManager.player,
            //playerName = playerInfo.playerName // Assuming PlayerInfo has a playerName field
        };
        // Convert game data to JSON string
        string json = JsonUtility.ToJson(gameData);
        // JSON string encoding
        string encryptedJson = EncryptionUtility.Encrypt(json);
        // Save to file
        string filePath = Path.Combine(Application.dataPath, "Resources", "savegame.json");

        File.WriteAllText(filePath, encryptedJson);
        Debug.Log("Game Saved!");
    }

    public GameData LoadGame()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources", "savegame.json");

        if (File.Exists(filePath))
        {
            //string json = File.ReadAllText(filePath);
            // Read from file
            string encryptedJson = File.ReadAllText(filePath);
            Debug.Log("EncryptedJSON: " + encryptedJson);
            // Decrypt data
            string decryptedJson = EncryptionUtility.Decrypt(encryptedJson);
            Debug.Log("DecryptedJSON: " + decryptedJson);
            GameData gameData = JsonUtility.FromJson<GameData>(decryptedJson);
            return gameData;
        }
        else
        {
            Debug.Log("No save file found.");
            return null;
        }
    }
    public void DeleteSaveGame()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources", "savegame.json");
        string metaFilePath = Path.Combine(Application.dataPath, "Resources", "savegame.json.meta");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Save game file has been deleted.");
        }
        else
        {
            Debug.LogWarning("No save game file found to delete.");
        }
        if (File.Exists(metaFilePath))
        {
            File.Delete(metaFilePath);
            Debug.Log("Save game meta file has been deleted.");
        }
        else
        {
            Debug.LogWarning("No save game meta file found to delete.");
        }
    }
    
}
