using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private Text _scoreText;

    private void Start()
    {
        _scoreText = GetComponent<Text>();
    }

    private void Awake()
    {
        ScoreManager.Instance.OnScoreChanged += UpdateScoreUI;
    }

    private void UpdateScoreUI(int score)
    {
        _scoreText.text = $"{score}";
    }
}
