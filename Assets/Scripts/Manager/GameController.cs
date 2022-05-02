using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum LevelState { Title, World1, GameOver };
    public LevelState state;

    public Canvas LoadingCanvas;
    public Canvas UICanvas;

    public LivesManager LM;
    public ScoreManager SM;
    public TimeManager TM;
    public CoinManager CM;

    private GameObject[] deleteIt;

    public Animator RestartMario;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        state = GetLevelState();
        LM = gameObject.GetComponent<LivesManager>();
        SM = gameObject.GetComponent<ScoreManager>();
        TM = gameObject.GetComponent<TimeManager>();
        CM = gameObject.GetComponent<CoinManager>();
        RestartMario = GameObject.Find("mario_small").GetComponent<Animator>();


    }

    // Start is called before the first frame update
    void Start()
    {
        if (state == LevelState.Title)
        {
            SetUpTitle();
        }
        TM.UnpauseTimer();

    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("GameController").Length > 1)
        {
            deleteIt = GameObject.FindGameObjectsWithTag("GameController");
            deleteIt[1].SetActive(false);
            LM.UpdateLivesText();
        }

    }

    private LevelState GetLevelState()
    {
        string levelName = SceneManager.GetActiveScene().name;
        if (levelName == "TitleScene")
        {
            return LevelState.Title;
        }
        else if (levelName == "World 1-1")
        {
            return LevelState.World1;
        }
        else if (levelName == "GameOverScene")
        {
            return LevelState.GameOver;
        }

        return LevelState.Title;
    }

    public void LoadLoadingScene()
    {
        
        LoadScene();
        ShowLoadingCanvas();
        TM.ToggleTimerText(false);
        ShowUICanvas();
        Invoke("LoadMainScene", 2f);
    }

    public void ShowLoadingCanvas()
    {
        LM.UpdateLivesText();
        LoadingCanvas.enabled = true;
    }

    private void LoadMainScene()
    {
        
        LoadScene();
        TM.UnpauseTimer();
        LoadingCanvas.enabled = false;
        TM.ToggleTimerText(true);
        ShowUICanvas();
        SoundGuy.Instance.PlaySound("main_theme", true);
    }

    private void LoadGameOverScene()
    {
        state = LevelState.Title;
        LoadScene();
        LoadingCanvas.enabled = false;
        ShowUICanvas();
        SoundGuy.Instance.PlaySound("smb_gameover");
        Invoke("LoadTitleScene", 4f);
    }

    public void LoadTitleScene()
    {
        state = LevelState.Title;
        foreach( GameObject g in GameObject.FindGameObjectsWithTag("Soundguy"))
        {
            Destroy(g);
        }
        LoadScene();
        SetUpTitle();
        Destroy(gameObject);
        
    }

    public void ShowUICanvas()
    {
        SM.UpdateScoreText();
        CM.UpdateCoinText();
        UICanvas.enabled = true;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene((int)state);

    }

    public void SetUpTitle()
    {
        ResetAllStats();
        ShowUICanvas();
        TM.ToggleTimerText(false);
        LoadingCanvas.enabled = false;

    }

    public void ResetAllStats()
    {
        TM.ResetTimer();
        SM.ResetScore();
        CM.ResetCoinCount();
        LM.ResetLives();
    }

    public void GameOver()
    {
        LoadGameOverScene();
    }

    public void DeadTest()
    {
        
        RestartMario.SetBool("isDead", false);
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector2(-27.53f, -9.8f);
        GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(-20,-9, -10);
        LM.UpdateLivesText();
        TM.ResetTimer();
        TM.UnpauseTimer();
        SoundGuy.Instance.PlaySound("main_theme", true);
    }

    public void MarioDie()
    {
        LM.LoseLives();
        TM.PauseTimer();
        CM.ResetCoinCount();
        SoundGuy.Instance.PlaySound("", true);
        SoundGuy.Instance.PlaySound("smb_mariodie", false);
        if (LM.CheckLives())
        {
            Invoke("DeadTest", 3f);
        }
        else
        {
            Invoke("GameOver", 3f);
        }
        
    }



    
}
