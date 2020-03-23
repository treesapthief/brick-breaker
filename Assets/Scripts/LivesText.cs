using UnityEngine;
using UnityEngine.UI;

public class LivesText : MonoBehaviour
{
    private Text _livesText;

    private void Start()
    {
        _livesText = GetComponent<Text>();
    }

    private void Awake()
    {
        LivesManager.Instance.OnLivesChanged += UpdateLivesUI;
    }

    private void UpdateLivesUI(int lives)
    {
        _livesText.text = $"Lives {lives}";
    }
}
