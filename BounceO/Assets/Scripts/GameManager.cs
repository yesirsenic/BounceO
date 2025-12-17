using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField]
    private BallMovement BallMovement;

    [SerializeField]
    private InputManager InputManager;

    [SerializeField]
    private GameObject NOAD;

    [SerializeField]
    private GameObject Start_Button;

    [SerializeField]
    private Text score_Text;

    [SerializeField]
    private Text BestScore_Text;

    [SerializeField]
    private GameObject NOAD_Button;

    [SerializeField]
    private GameObject NOAD_Popup;

    public enum GameState
    {
        Ready, InGame, End
    }

    private const int AD_INTERVAL = 2;

    public GameState state = GameState.Ready;
    public GameState prevState = GameState.Ready;
    public int Score = 0;
    public int BestScore = 0;
    public int deathCount = 0;

    public bool MusicOn = true;

    private void Update()
    {
        GameManage();
        ChangeScore();
        ChangeBestScore();

        RefreshUI();
    }

    private void GameManage()
    {
        if(state != prevState)
        {
            switch(state)
            {
                case GameState.Ready:
                    BallMovement.enabled = false;
                    InputManager.enabled = false;
                    NOAD.SetActive(true);
                    break;
                case GameState.InGame:
                    if (BallMovement.gameObject.activeSelf == false) { BallMovement.gameObject.SetActive(true); }
                    BallMovement.enabled = true;
                    InputManager.enabled = true;
                    NOAD.SetActive(false);
                    break;
                case GameState.End:
                    BallMovement.enabled = false;
                    InputManager.enabled = false;
                    NOAD.SetActive(true);
                    break;
            }

            prevState = state;
        }
    }

    private void RefreshUI()
    {
        if (NOAD_Button.activeSelf == false)
            return;

        bool noAds = PlayerPrefs.GetInt("NO_ADS", 0) == 1;
        NOAD_Button.gameObject.SetActive(!noAds);
        NOAD_Popup.gameObject.SetActive(!noAds);
    }

    public void ChangeState(GameState changestate)
    {
        state = changestate;
    }

    public void SetStateReady()
    {
        ChangeState(GameState.Ready);
    }

    public void SetStateInGame()
    {
        ChangeState(GameState.InGame);
    }

    public void SetStateEnd()
    {
        ChangeState(GameState.End);
    }

    public void ChangeScore()
    {
        score_Text.text = Score.ToString(); 
    }

    public void ChangeBestScore()
    {
        if(Score > BestScore)
        {
            BestScore = Score;
        }

        BestScore_Text.text = BestScore.ToString();
    }

    public void GameEnd()
    {
        if(BestScore <= Score)
        {
            PlayerPrefs.SetInt("BestScore", Score);
        }

        state = GameState.End;

        Start_Button.SetActive(true);

        deathCount++;

        if(deathCount >= AD_INTERVAL)
        {
            deathCount = 0;

            AdManager.Instance.ShowInterstitial();
        }

    }

    public void __Init__()
    {
        BestScore = PlayerPrefs.GetInt("BestScore");
        BallMovement.InitSpeed();
        Score = 0;
    }
}
