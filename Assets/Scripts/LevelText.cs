using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    private Text _levelText;
    
    private void Awake()
    {
        Debug.Log("LevelText.Awake");
        LevelManager.Instance.OnLevelChanged += UpdateLevelUI;
    }

    private void UpdateLevelUI(int level)
    {
        Debug.Log($"LevelText.UpdateLevelUI ({level})");
        _levelText = GetComponent<Text>();
        _levelText.text = $"Level {level}";
    }
}
