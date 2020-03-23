using UnityEngine;

public delegate void OnScoreChangedHandler(int score);

public class ScoreManager
{
    public event OnScoreChangedHandler OnScoreChanged;
    
    private int _playerScore;
    private static ScoreManager _instance = null;


    protected ScoreManager()
    {
    }

    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ScoreManager();
            }

            return _instance;
        }
    }

    public void GivePoints(int points)
    {
        SetScore(_playerScore + points);
    }

    private void SetScore(int score)
    {
        _playerScore = score;
        OnScoreChanged?.Invoke(_playerScore);
    }

    public void Reset()
    {
        Debug.Log("ScoreManager.Reset");
        SetScore(0);
    }
}