using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject BrickTemplate;
    public Text LevelText;
    public Vector3 Offset;
    public int LevelWidth = 45;
    public int LevelHeight = 24;
    public int BrickWidth = 4;
    public int BrickHeight = 2;
    public int MaxBricks = 120;
    private static LevelManager _instance = null;
    private int _brickCount = 0;
    private int _currentLevel = 1;

    public LevelManager()
    {
    }

    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LevelManager();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        GameManager.Instance.OnStateChange += LevelCompleted;
        BuildLevel(1);
    }

    public void BuildLevel(int level)
    {
        LevelText.text = $"Level {level}";
        _brickCount = 0;
        for (var w = 0; w < LevelWidth; w+=BrickWidth) {
            for (var h = 0; h < LevelHeight; h+=BrickHeight)
            {
                var position = new Vector3(w + Offset.x, h + Offset.y);
                Instantiate(BrickTemplate, position, Quaternion.identity);
                _brickCount++;

                if (_brickCount >= MaxBricks)
                {
                    break;
                }
            }

            if (_brickCount >= MaxBricks)
            {
                break;
            }
        }

        GameManager.Instance.SetGameState(GameState.WaitForStart);
    }

    public void RemoveBricks(int count)
    {
        _brickCount -= count;
        if (_brickCount <= 0)
        {
            _brickCount = 0;
            GameManager.Instance.SetGameState(GameState.LevelComplete);
        }
    }



    private void LevelCompleted(GameState newState)
    {
        if (newState == GameState.LevelComplete)
        {
            _currentLevel++;
            LevelText.text = $"Level {_currentLevel}";
            BuildLevel(_currentLevel);
            // TODO: Load the next level
            // BUG: Will this race for condition in the other events?
        }
    }
}
