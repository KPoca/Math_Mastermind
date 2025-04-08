using Unity.Properties;
using UnityEngine;

public class PlayerInfo
{
    public static int difficulty;
    public static float spawnRate;
    //public Problem problem;
    public static float timePerProblem;
    public static float remainingTime;  //recently added
    public static int numberOfProblems;
    public static int curProblem;       //recently added
    public static int score;
    public static LevelSettings levelSettings;
    public static bool isWin;           //recently added

    //Singleton instance
    public static PlayerInfo instance; 
    public PlayerInfo()
    {
        difficulty = 1;
        score = 0;
        spawnRate = 1;
        timePerProblem = 60;
        numberOfProblems = 10;
        levelSettings = null;
    }
    // Static property to access the instance
    public static PlayerInfo Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerInfo();

                // If you need to keep the instance alive throughout the game (for GameObject), you can use DontDestroyOnLoad() here.
                // For example, if you need PlayerInfo to be a GameObject in the scene, you can create a GameObject containing PlayerInfo and call DontDestroyOnLoad.

            }
            return instance;
        }
    }
}
