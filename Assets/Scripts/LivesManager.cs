using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public int StartingLives = 3;
    public int MaxLives = 9;

    private int _currentLives;

    private void Start()
    {
        _currentLives = StartingLives;
    }

    internal void TakeLives(int numberOfLives)
    {
        _currentLives -= numberOfLives;
        if (_currentLives <= 0)
        {
            _currentLives = 0;
            GameManager.Instance.SetGameState(GameState.GameOver);
        }
    }

    internal void AddLives(int numberOfLives)
    {
        _currentLives += numberOfLives;
        if (_currentLives >= MaxLives)
        {
            _currentLives = MaxLives;
        }
    }
}
