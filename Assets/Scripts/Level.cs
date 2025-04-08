using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class Level : MonoBehaviour
{
    //static int count = 0;
    //public PlayerInfo playerInfo;
    public LevelSettings levelSettings;
    public SaveGameManager saveGameManager;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        //audioManager = AudioManager.instance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if (playerInfo == null)
        //{
        //    PlayerInfo playerInfo = new PlayerInfo();
        //}
        //Debug.Log(levelSettings);
        //Debug.Log(GetComponent<LevelSettings>());
        
    }
    // Update is called once per frame 
    void Update()
    {
        
    }

    public Level()
    {
        //    Debug.Log(count++ + " " + levelSettings);
        //    if (levelSettings != null)
        //    {
        //        PlayerInfo.difficulty = levelSettings.difficulty;
        //        PlayerInfo.spawnRate = levelSettings.spawnRate;
        //        PlayerInfo.timePerProblem = levelSettings.timePerProblem;
        //        PlayerInfo.numberOfProblems = levelSettings.numberOfProblems;
        //        PlayerInfo.levelSettings = levelSettings;

        //        Debug.Log("Level settings have been applied to PlayerInfo");
        //        Debug.Log(PlayerInfo.difficulty + " " + PlayerInfo.spawnRate + " " + PlayerInfo.timePerProblem + " " + PlayerInfo.numberOfProblems);

        //    }
        //    else
        //    {
        //        Debug.Log("Really man?");
        //        Debug.LogError("LevelSettings is not assigned!");
        //    }
    }
    public void LoadLevelSettings()
    {
        LevelSettings levelSettings = Resources.Load<LevelSettings>($"lv{PlayerInfo.difficulty}Settings");
        if (levelSettings != null)
        {
            PlayerInfo.difficulty = levelSettings.difficulty;
            PlayerInfo.spawnRate = levelSettings.spawnRate;
            PlayerInfo.timePerProblem = levelSettings.timePerProblem;
            PlayerInfo.numberOfProblems = levelSettings.numberOfProblems;
            //PlayerInfo.levelSettings = levelSettings;
            Debug.Log($"Loaded settings for level {PlayerInfo.difficulty}");
            Debug.Log(PlayerInfo.difficulty + " " + PlayerInfo.spawnRate + " " + PlayerInfo.timePerProblem + " " + PlayerInfo.numberOfProblems);
        }
        else
        {
            Debug.LogError("Failed to load level settings.");
        }
    }

    public void NextLevel()
    {
        audioManager.PlaySFX(audioManager.click);
        //saveGameManager = FindAnyObjectByType<SaveGameManager>();
        //Debug.Log("Get into NextLevel");
        ++PlayerInfo.difficulty;
        LoadLevelSettings();

        PlayerInfo.curProblem = 0;
        PlayerInfo.score = GameManager.instance.player.score;
        
        saveGameManager.SaveGame();
        SceneManager.LoadSceneAsync(2);
    }
    public void ReturnHome()
    {
        audioManager.PlaySFX(audioManager.click);
        if (PlayerInfo.isWin == true)
        {
            ++PlayerInfo.difficulty;
            LoadLevelSettings();
        }
        PlayerInfo.score = GameManager.instance.player.score;
        saveGameManager.SaveGame();
        SceneManager.LoadSceneAsync(0);
    }
    public void Replay()
    {
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadSceneAsync(2);
    }
    public void StartGame()
    {
        audioManager.PlaySFX(audioManager.click);
        //Debug.Log("Level Button onClicked");
        PlayerInfo.difficulty = levelSettings.difficulty;
        PlayerInfo.spawnRate = levelSettings.spawnRate;
        PlayerInfo.timePerProblem = levelSettings.timePerProblem;
        PlayerInfo.numberOfProblems = levelSettings.numberOfProblems;
        //PlayerInfo.levelSettings = levelSettings;

        PlayerInfo.curProblem = 0;

        Debug.Log("Level settings have been applied to PlayerInfo");
        Debug.Log(PlayerInfo.difficulty + " " + PlayerInfo.spawnRate + " " + PlayerInfo.timePerProblem + " " + PlayerInfo.numberOfProblems);
        PlayerInfo.score = 0;
        SceneManager.LoadSceneAsync(2);

        //LevelSettings newSettings = new LevelSettings { timePerProblem = 5, difficulty = 1, spawnRate = 1, numberOfProblems = 10 };
        //Debug.Log(newSettings);
        //GameManager.Instance.Initialize(newSettings);
        //GameManager.Instance.StartGame();

        //StartCoroutine(LoadGameSceneAsync());

        //gameManager.Initialize(levelSettings); 

        //gameManager.StartGame();
        //if (gameManager == null)
        //{
        //    Debug.LogError("GameManager chua duoc gan!");
        //    return;
        //}
        //StartCoroutine(LoadGameSceneAsync());


        //gameManager.plusScore(10);  
        ////GameManager.instance.plusScore(10);
        //SceneManager.LoadSceneAsync(2);
        //Debug.Log("PlayGame");
        //Debug.Log(levelSettings);
        //if (levelSettings != null)
        //{
        //    Debug.Log($"Difficulty: {levelSettings.difficulty}, SpawnRate: {levelSettings.spawnRate}, TimePerProblem: {levelSettings.timePerProblem}");
        //    GameManager.instance.setCurLvSettings(levelSettings);
        //    Debug.Log("Prepare to load scene");
        //    SceneManager.LoadSceneAsync(2);
        //}
        //else
        //{
        //    Debug.Log("Fail");
        //    Debug.LogError("Cannot apply LevelSettings!");
        //}
    }
    private IEnumerator LoadGameSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
