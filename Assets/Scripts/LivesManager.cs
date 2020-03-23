using UnityEngine;

public delegate void OnLivesChangedHandler(int lives);

public class LivesManager : MonoBehaviour
{
    public event OnLivesChangedHandler OnLivesChanged;
    public int StartingLives = 3;
    public int MaxLives = 9;

    private int _currentLives;
    private static LivesManager _instance = null;


    private void Start()
    {
        SetLives(StartingLives);
    }

    public static LivesManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LivesManager();
            }

            return _instance;
        }
    }


    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void OnApplicationQuit()
    {
        _instance = null;
    }

    private void SetLives(int lives)
    {
        _currentLives = lives;

        if (_currentLives <= 0)
        {
            _currentLives = 0;
            GameManager.Instance.SetGameState(GameState.GameOver);
        }

        if (_currentLives >= MaxLives)
        {
            _currentLives = MaxLives;
        }

        OnLivesChanged?.Invoke(_currentLives);
    }

    internal void TakeLives(int numberOfLives)
    {
        SetLives(_currentLives - numberOfLives);
    }

    internal void AddLives(int numberOfLives)
    {
        SetLives(_currentLives + numberOfLives);
    }
}
