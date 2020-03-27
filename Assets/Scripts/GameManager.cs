using UnityEngine;

public enum GameState
{
    NewGame,
    WaitForStart,
    InGame,
    Paused,
    GameOver,
    LevelComplete
}

public delegate void OnStateChangeHandler(GameState newState);

public delegate void OnWaitToStartHandler();

public class GameManager : MonoBehaviour
{
    public event OnStateChangeHandler OnStateChange;
    public event OnWaitToStartHandler OnRestartLevel;
    public GameState GameState { get; private set; }

    private static GameManager _instance = null;

    protected GameManager()
    {
        _instance = this;
    }

    public void SetGameState(GameState state)
    {
        Debug.Log($"GameState changed: {state}");
        GameState = state;
        OnStateChange?.Invoke(state);
        if (GameState == GameState.WaitForStart)
        {
            OnRestartLevel?.Invoke();
        }
    }

    private void Start()
    {
        Debug.Log("GameManager.Start");
        LivesManager.Instance.SetLives(LivesManager.Instance.StartingLives);
        SetGameState(GameState.NewGame);
    }

    private void Update()
    {
        if (GameState == GameState.NewGame && Input.anyKeyDown)
        {
            SetGameState(GameState.WaitForStart);
        }
        else if (GameState == GameState.WaitForStart && Input.anyKeyDown)
        {
            SetGameState(GameState.InGame);
        }
        else if (GameState == GameState.GameOver && Input.anyKeyDown)
        {
            SetGameState(GameState.NewGame);
            ScoreManager.Instance.Reset();
            LivesManager.Instance.SetLives(LivesManager.Instance.StartingLives);
        }
    }

    public static GameManager Instance
    {
        get
        {
            //if (_instance == null)
            //{
            //    _instance = new GameManager();
            //}

            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        _instance = null;
    }
}