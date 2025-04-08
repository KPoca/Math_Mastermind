using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //public GameObject levelPanel;
    //public GameObject mainPanel;
    //public GameObject levelButton;
    //public GameObject levelButtonPrefab;
    //public LevelSettings[] levels;
    //public GameObject levelPanelPrefab;
    //public GameObject levelButtonPanel;
    //public GameObject levelButtonPanelPrefab;
    //public GameObject levelButtonPanelParent;
    //public GameObject levelButtonPanelParentPrefab;
    //public GameObject levelButtonPanelParentParent;
    //public GameObject levelButtonPanelParentParentPrefab;
    //public GameObject levelButtonPanelParentParentParent;

    //public static MainMenu instance; // Design pattern: Singleton
    public SaveGameManager saveGameManager;
    public Button continueBtn;
    public Button settingsBtn;
    public Button exitBtn;
    private SaveGameManager.GameData gameData;
    AudioManager audioManager;
    void Awake()
    {
        //audioManager = AudioManager.instance;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        //Debug.Log(Equals(audioManager, AudioManager.instance));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //saveGameManager = FindAnyObjectByType<SaveGameManager>();
        Debug.Log("Game Data: " + gameData);
        gameData = saveGameManager.LoadGame();
        
        if (gameData != null)
        {
            continueBtn.gameObject.SetActive(true);
        }
        else
        {
            continueBtn.gameObject.SetActive(false);
            RectTransform rectTransform = settingsBtn.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0f, 14f);
            rectTransform = exitBtn.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0f, -54f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Click event for the Play button
    public void PlayGame()
    {
        audioManager.PlaySFX(audioManager.click);
        //SceneManager.LoadScene("Game");
        Debug.Log("Prepare to delete SaveGame");
        saveGameManager.DeleteSaveGame();
        SceneManager.LoadSceneAsync(1);
    }
    public void ContinueGame()
    {
        audioManager.PlaySFX(audioManager.click);
        //GameManager gameManager = FindObjectOfType<GameManager>();
        PlayerInfo.numberOfProblems = gameData.numberOfProblems;
        PlayerInfo.curProblem = gameData.curProblem;
        PlayerInfo.timePerProblem = gameData.timePerProblem;
        PlayerInfo.remainingTime = gameData.remainingTime;
        PlayerInfo.difficulty = gameData.difficulty;
        PlayerInfo.spawnRate = gameData.spawnRate;
        PlayerInfo.isWin = gameData.isWin;
        PlayerInfo.score = gameData.score;
        //if (PlayerInfo.isWin)
        //{
        //    PlayerInfo.difficulty++;
            
        //}
        //gameManager.playerName = gameData.playerName;
        SceneManager.LoadSceneAsync(2);
    }
    public void Settings()
    {
        audioManager.PlaySFX(audioManager.click);
        SceneManager.LoadScene("Settings");
    }
    // Click event for the Quit button
    public void Quit()
    {
        audioManager.PlaySFX(audioManager.click);
        Application.Quit(); 
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player"))
        //{
        //    PlayGame();
        //}
        //if (collision.CompareTag("Player"))
        //{
        //    Quit();
        //}
        if (collision.GetComponent<PlayerController>())
        {
            PlayGame();
        }
        else
        {
            Debug.Log("Exiting...");
            Application.Quit();
        }
    }
}
