using UnityEngine;
using UnityEngine.UI;

public class LivesText : MonoBehaviour
{
    private Text _livesText;

    private void Awake()
    {
        LivesManager.Instance.OnLivesChanged += UpdateLivesUI;
    }

    private void UpdateLivesUI(int lives)
    {
        Debug.Log("LivesText.UpdateLivesUI");
        _livesText = GetComponent<Text>();
        _livesText.text = $"Lives {lives}";
    }
}
