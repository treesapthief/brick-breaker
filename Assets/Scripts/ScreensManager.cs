using UnityEngine;

public class ScreensManager : MonoBehaviour
{
    public GameObject GameOverScreen;
    public GameObject WaitToStartScreen;

    private void Awake()
    {
        GameManager.Instance.OnStateChange += OnStateChanged;
    }

    private void OnStateChanged(GameState newState)
    {
        GameOverScreen.SetActive(false);
        WaitToStartScreen.SetActive(false);
        if (newState == GameState.GameOver)
        {
            GameOverScreen.SetActive(true);
        }
        else if (newState == GameState.WaitForStart)
        {
            WaitToStartScreen.SetActive(true);
        }
    }
}
