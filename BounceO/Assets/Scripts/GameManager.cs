using UnityEngine;

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

    public enum GameState
    {
        Ready, InGame, End
    }

    public GameState state = GameState.Ready;
    public GameState prevState = GameState.Ready;

    private void Update()
    {
        GameManage();
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
                    break;
                case GameState.InGame:
                    BallMovement.enabled = true;
                    InputManager.enabled = true;
                    break;
                case GameState.End:
                    BallMovement.enabled = false;
                    InputManager.enabled = false;
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
}
