using UnityEngine;
using UnityEngine.UI;

public class LivesText : MonoBehaviour
{
    private Text _livesText;

    private void Start()
    {
        Debug.Log("LivesText.Start");
        _livesText = GetComponent<Text>();
        LivesManager.Instance.OnLivesChanged += UpdateLivesUI;
    }

    private void Awake()
    {
        Debug.Log("LivesText.Awake");
    }

    private void UpdateLivesUI(int lives)
    {
        Debug.Log("LivesText.UpdateLivesUI");
        _livesText.text = $"Lives {lives}";
    }
}
