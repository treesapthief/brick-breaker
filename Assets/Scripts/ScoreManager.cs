using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public GameObject PlayerScoreText;

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
        _playerScore += points;
        SetText(_playerScore);
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
    

    public void Reset()
    {
        Debug.Log("ScoreManager.Reset");
        _playerScore = 0;
        SetText(_playerScore);
    }

    private void SetText(int score)
    {
        var text = PlayerScoreText.GetComponent<Text>();
        text.text = score.ToString();
        // TODO: How do I set this independently? Event when _playerScore changes?
    }
}