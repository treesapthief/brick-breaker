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

public class GameManager : MonoBehaviour
{
    public event OnStateChangeHandler OnStateChange;
    public GameState GameState { get; private set; }
    public ScoreManager ScoreManager { get; private set; }
    public LevelManager LevelManager { get; private set; }


    private static GameManager _instance = null;

    protected GameManager()
    {
        GameState = GameState.NewGame;
    }

    private void Start()
    {
        ScoreManager = GetComponent<ScoreManager>();
        LevelManager = GetComponent<LevelManager>();
//        LevelManager.Instance.BuildLevel(1);
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
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }

            return _instance;
        }
    }


    public void SetGameState(GameState state)
    {
        Debug.Log($"GameState changed: {state}");
        GameState = state;
        OnStateChange?.Invoke(state);
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