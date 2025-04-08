using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor.Build.Player;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SaveGameManager;
public class GameManager : MonoBehaviour
{
    private IProblemFactory problemFactory;  // Biến lưu trữ factory thông qua interface
    public void SetProblemFactory(IProblemFactory factory)  // Setter để tiêm factory vào
    {
        problemFactory = factory;
    }
    //public Problem[] problems;      // list of all problems
    public int numberOfProblems;
    public int curProblem;          // current problem the player needs to solve
    public Problem problem;
    public float timePerProblem;    // time allowed to answer each problem

    public float remainingTime;     // time remaining for the current problem

    public PlayerController player; // player object
    public Button nextLevelBtn;
    public Button menuBtn;
    public Button replayBtn;
    public FadeOutAnimationText levelText;
    public int difficulty;
    private bool isGamePaused = false;
    //public SaveGameManager saveGameManager;
    AudioManager audioManager;
    // instance
    public static GameManager instance; // Design pattern: Singleton
    //public LevelSettings curLvSettings; // level settings
    //public static GameManager Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = Object.FindFirstObjectByType<GameManager>(); // Dùng FindFirstObjectByType thay cho FindObjectOfType
    //            if (instance == null)
    //            {
    //                // Nếu không tìm thấy, tạo mới một GameObject và gắn GameManager vào
    //                GameObject go = new GameObject("GameManager");
    //                instance = go.AddComponent<GameManager>();
    //            }
    //        }
    //        return instance;
    //    }
    //}

    //void Awake ()
    //{
    //    //// set instance to this script.
    //    //instance = this;
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject); // Giữ GameManager sống qua các scene
    //    }
    //    else
    //    {
    //        Destroy(gameObject); // Nếu đã có instance, hủy GameManager mới
    //    }
    //}

    void Awake()
    {
        instance = this;
        if (PlayerInfo.difficulty == 0)
        {
            // Debug circumstance
            PlayerInfo.difficulty = 1;
            PlayerInfo.spawnRate = 1;
            PlayerInfo.timePerProblem = 60;
            PlayerInfo.numberOfProblems = 10;
            PlayerInfo.levelSettings = null;
            remainingTime = timePerProblem;
        }
        else
        {
            //ObstacleSpawner.instance.spawnRate = PlayerInfo.spawnRate;
            timePerProblem = PlayerInfo.timePerProblem;
            difficulty = PlayerInfo.difficulty;
            numberOfProblems = PlayerInfo.numberOfProblems;
            //Debug.Log("number of problems : " + numberOfProblems);
            player.score = PlayerInfo.score;
            
        }
        audioManager = AudioManager.instance;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        Debug.Log(Equals(audioManager, AudioManager.instance));
    }

    void Start()
    {
        // set the initial problem
        //SetProblem(0);

        //if (curLvSettings != null)
        //{
        //    Debug.Log(curLvSettings.timePerProblem + " " + curLvSettings.difficulty);
        //    ApplyLvSettings(curLvSettings);
        //    //difficulty = curLvSettings.difficulty;
        //    //Debug.Log(difficulty);
        //}
        
        levelText.text.text = "Level " + difficulty;
        levelText.StartCoroutine(levelText.ShowText());
        PlayerInfo.isWin = false;
        Time.timeScale = 1.0f;
        //curProblem = 1;
        curProblem = PlayerInfo.curProblem;
        //nextLevelBtn.onClick.AddListener(onNextLevelBtnClicked);
        Debug.Log("number of problems: " + numberOfProblems);
        if (difficulty != 6)
        {
            ProgressBar.instance.maximumProgress = numberOfProblems;
        }
        else
        {
            ProgressBar.instance.maximumProgress = 9999;
        }

        nextLevelBtn.gameObject.SetActive(false);
        menuBtn.gameObject.SetActive(false);
        replayBtn.gameObject.SetActive(false);
        //menuBtn.onClick.AddListener(() => SceneManager.LoadSceneAsync(0));

        // Khởi tạo Factory
        IProblemFactory factory = new ProblemFactory();

        // Tiêm factory vào GameManager
        SetProblemFactory(factory);
        genProblem();

        UI.instance.SetScoreText(player.score);
        Debug.Log(LoadHighScore()); //Debug
        UI.instance.SetHighScoreText(LoadHighScore());
        //// Gắn sự kiện cho các nút trả lời
        //for (int i = 0; i < 4; i++)
        //{
        //    int index = i;
        //    answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        //}
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (remainingTime != 0)
            {
                if (isGamePaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
        if (difficulty != 6)
        {
            ProgressBar.instance.currentProgress = curProblem;
        }
        else
        {
            ProgressBar.instance.currentProgress = player.score;
        }
        remainingTime -= Time.deltaTime;

        // has the remaining time ran out?
        if (remainingTime <= 0.0f)
        {
            Lose();
        }
    }
    public void UpdateHighScore(int curScore)
    {
        string filePath = Path.Combine(Application.dataPath, "Resources", "highscore.json");
        if (File.Exists(filePath))
        {
            string encryptedJson = File.ReadAllText(filePath);
            string decryptedJson = EncryptionUtility.Decrypt(encryptedJson);
            GameData gameData = JsonUtility.FromJson<GameData>(decryptedJson);
            if (curScore > gameData.score)
            {
                gameData.score = curScore;
                string json = JsonUtility.ToJson(gameData);
                string encryptedJson2 = EncryptionUtility.Encrypt(json);
                File.WriteAllText(filePath, encryptedJson2);
                //return curScore;
            }
            //return gameData.score;

        }
        else
        {
            GameData gameData = new GameData()
            {
                score = curScore
            };
            string json = JsonUtility.ToJson(gameData);
            string encryptedJson = EncryptionUtility.Encrypt(json);
            File.WriteAllText(filePath, encryptedJson);
            //return curScore;
        }
    }
    public int LoadHighScore()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources", "highscore.json");
        if (File.Exists(filePath))
        {
            string encryptedJson = File.ReadAllText(filePath);
            string decryptedJson = EncryptionUtility.Decrypt(encryptedJson);
            GameData gameData = JsonUtility.FromJson<GameData>(decryptedJson);
            return gameData.score;
        }
        else
        {
            return 0;
        }
    }
    void PauseGame()
    {
        Time.timeScale = 0f; 
        isGamePaused = true;

        UI.instance.SetPauseGameText();
        if (levelText != null)
        {
            levelText.EndShowText();
        }

        RectTransform rectTransform = menuBtn.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-40f, -65f);
        rectTransform = replayBtn.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(40f, -65f);
        menuBtn.gameObject.SetActive(true);
        replayBtn.gameObject.SetActive(true);

    }

    // Hàm tiếp tục game
    void ResumeGame()
    {
        Time.timeScale = 1f; 
        isGamePaused = false;
        UI.instance.UnsetPauseGameText();
        levelText.StartCoroutine(levelText.ShowText());
        menuBtn.gameObject.SetActive(false);
        replayBtn.gameObject.SetActive(false);
    }
    void genProblem()
    {
        if (problemFactory != null)
        {
            problem = problemFactory.CreateProblem();  // Sử dụng factory để tạo câu hỏi
            remainingTime = timePerProblem;
            UI.instance.SetProblemText(problem);
        }
        else
        {
            Debug.LogError("ProblemFactory is not set!");
        }
        //// Tạo câu hỏi ngẫu nhiên
        //problem = new Problem();
        //UI.instance.SetProblemText(problem);
        //remainingTime = timePerProblem;
    }
    public void plusScore(int hscore)
    {
        player.score += hscore;
        // Score limit at 9999
        if (player.score > 9999)
        {
            player.score = 9999;
            UI.instance.SetScoreText(player.score);
            Win();
        }
        //PlayerInfo.score = player.score;    // Update PLayerInfo
        UI.instance.SetScoreText(player.score);
    }
    public void minusScore(int hscore)
    {
        player.score -= hscore;
        if (player.score < 0)
            player.score = 0;
        //PlayerInfo.score = player.score;    // Update PLayerInfo
        UI.instance.SetScoreText(player.score);
    }

    // called when the player enters a tube
    public void OnPlayerEnterTube(int tube)
    {
        // did they enter the correct tube?
        //if (tube == problems[curProblem].correctTube)
        if (tube == problem.correctTube)
        {
            audioManager.PlaySFX(audioManager.rightAnswer);
            CorrectAnswer();
        }
        else
        {
            audioManager.PlaySFX(audioManager.wrongAnswer);
            IncorrectAnswer();
        }
            
    }

    // called when the player enters the correct tube
    void CorrectAnswer()
    {
        //// is this the last problem?
        //if(problems.Length - 1 == curProblem)
        //    Win();
        //else
        //    SetProblem(curProblem + 1);

        //Need to destroy current problem

        
        //Debug.Log(remainingTime + timePerProblem + (float)(remainingTime/ timePerProblem));
        int bonusScore = (int)Mathf.Round(50 * (float)(remainingTime / timePerProblem));
        //int difficulty = 3;
        plusScore(difficulty * bonusScore);
        curProblem++;
        if (curProblem < numberOfProblems)
        {
            //curProblem++;
            genProblem();
        }
        else
        {
            PlayerInfo.score = player.score;
            Win();
        }
    }

    // called when the player enters the incorrect tube
    void IncorrectAnswer()
    {
        minusScore(10);
        player.Stun();
    }

    //// sets the current problem
    //void SetProblem (int problem)
    //{
    //    curProblem = problem;
    //    UI.instance.SetProblemText(problems[curProblem]);
    //    remainingTime = timePerProblem;
    //}

    // called when the player answers all the problems
    void Win()
    {
        audioManager.PlaySFX(audioManager.win);
        UpdateHighScore(player.score);
        if (levelText != null)
        {
            levelText.EndShowText();
        }
        //PlayerInfo.score = player.score;
        //levelText.EndText();
        PlayerInfo.isWin = true;
        Time.timeScale = 0.0f;
        if (difficulty == 6)
        {
            UI.instance.SetWinGameText();
            RectTransform rectTransform = menuBtn.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(-40f, -65f);
            //rectTransform = replayBtn.GetComponent<RectTransform>();
            //rectTransform.anchoredPosition = new Vector2(40f, -65f);
            rectTransform = replayBtn.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(40f, -65f);
        }
        else
        {
            UI.instance.SetEndText(true);
            nextLevelBtn.gameObject.SetActive(true);
        }
        menuBtn.gameObject.SetActive(true);
        replayBtn.gameObject.SetActive(true);
        

        //LevelSettings levelSettings = Resources.Load<LevelSettings>($"lv{++difficulty}Settings");
        //if (levelSettings != null)
        //{
        //    PlayerInfo.difficulty = levelSettings.difficulty;
        //    PlayerInfo.spawnRate = levelSettings.spawnRate;
        //    PlayerInfo.timePerProblem = levelSettings.timePerProblem;
        //    PlayerInfo.numberOfProblems = levelSettings.numberOfProblems;
        //    PlayerInfo.levelSettings = levelSettings;
        //    Debug.Log($"Loaded settings for level {difficulty}");
        //}
        //else
        //{
        //    Debug.LogError("Failed to load level settings.");
        //}
        //StartCoroutine(WaitForInput());
    }

    // called if the remaining time on a problem reaches 0
    void Lose()
    {
        UpdateHighScore(player.score);
        audioManager.PlaySFX(audioManager.gameOver);
        Time.timeScale = 0.0f;
        UI.instance.SetEndText(false);
        RectTransform rectTransform = menuBtn.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(-40f, -65f);
        rectTransform = replayBtn.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(40f, -65f);
        menuBtn.gameObject.SetActive(true);
        replayBtn.gameObject.SetActive(true);

    }

    //private IEnumerator WaitForInput()
    //{
    //    // Chờ người chơi nhấn phím Enter hoặc Esc
    //    while (true)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Return))  // Kiểm tra phím Enter
    //        {
    //            // Load màn tiếp theo (có thể là level tiếp theo)
    //            LoadNextLevel();
    //            yield break;  // Dừng coroutine sau khi đã load scene
    //        }
    //        else if (Input.GetKeyDown(KeyCode.Escape))  // Kiểm tra phím Esc
    //        {
    //            // Load màn hình trang chủ
    //            SceneManager.LoadSceneAsync(0);
    //            yield break;  // Dừng coroutine sau khi đã load scene
    //        }

    //        yield return null;  // Chờ cho đến lần kiểm tra tiếp theo
    //    }
    //}
    //public void onClicked()
    //{
    //    Debug.Log("onClicked()");
    //}
    //private void onNextLevelBtnClicked()
    //{
    //    Debug.Log("onNextLevelBtnClicked()");
    //    LoadNextLevel();
    //}
    //public void LoadNextLevel()
    //{
    //    Debug.Log("onLoadNextLevel()");
    //    difficulty++;
    //    LoadLevelSettings(difficulty);
    //    SceneManager.LoadSceneAsync(2);
    //}
    //private void LoadLevelSettings(int level)
    //{
    //    LevelSettings levelSettings = Resources.Load<LevelSettings>($"lv{level}Settings");
    //    if (levelSettings != null)
    //    {
    //        PlayerInfo.difficulty = levelSettings.difficulty;
    //        PlayerInfo.spawnRate = levelSettings.spawnRate;
    //        PlayerInfo.timePerProblem = levelSettings.timePerProblem;
    //        PlayerInfo.numberOfProblems = levelSettings.numberOfProblems;
    //        PlayerInfo.levelSettings = levelSettings;
    //        Debug.Log($"Loaded settings for level {level}");
    //    }
    //    else
    //    {
    //        Debug.LogError("Failed to load level settings.");
    //    }
    //}
}