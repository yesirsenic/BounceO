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

    public enum GameState
    {
        Ready, InGame, End
    }

    public GameState state = GameState.Ready;
    public GameState prevState = GameState.Ready;
    public int Score = 0;
    public int BestScore = 0;

    public bool MusicOn = true;

    private void Update()
    {
        GameManage();
        ChangeScore();
        ChangeBestScore();
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

        //여기에 3회 시 광고 이런거 넣을 듯

    }

    public void __Init__()
    {
        BestScore = PlayerPrefs.GetInt("BestScore");
        BallMovement.InitSpeed();
        Score = 0;
    }
}
