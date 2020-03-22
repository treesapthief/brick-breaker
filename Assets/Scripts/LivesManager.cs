using UnityEngine;
using UnityEngine.UI;

public class LivesManager : MonoBehaviour
{
    public int StartingLives = 3;
    public int MaxLives = 9;
    public Text LivesText;

    private int _currentLives;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _currentLives = StartingLives;
        SetText(_currentLives);
    }

    internal void TakeLives(int numberOfLives)
    {
        _currentLives -= numberOfLives;
        if (_currentLives <= 0)
        {
            _currentLives = 0;
            GameManager.Instance.SetGameState(GameState.GameOver);
        }

        SetText(_currentLives);
    }

    internal void AddLives(int numberOfLives)
    {
        _currentLives += numberOfLives;
        if (_currentLives >= MaxLives)
        {
            _currentLives = MaxLives;
        }

        SetText(_currentLives);
    }

    private void SetText(int lives)
    {
        LivesText.text = $"Lives {lives}";
    }
}
