using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    private Text _levelText;

    private void Start()
    {
        _levelText = GetComponent<Text>();
    }

    private void Awake()
    {
        // TODO: This instance is "null", so it isn't working right
        GameManager.Instance.LevelManager.OnLevelChanged += UpdateLevelUI;
    }

    private void UpdateLevelUI(int level)
    {
        _levelText.text = $"Level {level}";
    }
}
