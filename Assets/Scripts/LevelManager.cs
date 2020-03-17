using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public GameObject BrickTemplate;
    public Text LevelText;
    public Vector3 Offset;
    public int MaxLevelWidth = 45;
    public int MaxLevelHeight = 24;
    public int BrickWidth = 4;
    public int BrickHeight = 2;
    public int MaxBricks = 120;
    public int NumberOfLevels = 1;
    private static LevelManager _instance = null;
    private int _brickCount = 0;
    private int _currentLevel = 1;

    public LevelManager()
    {
    }

    public static LevelManager Instance
    {
        get
        {
            //if (_instance == null)
            //{
            //    _instance = new LevelManager();
            //}

            return _instance;
        }
    }

    private void Awake()
    {
        GameManager.Instance.OnStateChange += LevelCompleted;
    }

    public void BuildLevel(int level)
    {
        LevelText.text = $"Level {_currentLevel}";
        _brickCount = 0;
        var levelData = GetLevel(level);

        // TODO: Validate level size (must be > 0 but equal or smaller to the Max Height/Width

        if (!CheckIfValidLevelData(levelData))
        {
            Debug.Log("Invalid level data");
            return;
        }

        for (var h = 0; h < Math.Min(levelData.Length, MaxLevelHeight); h++) {
            for (var w = 0; w < Math.Min(levelData[h].Length, MaxLevelWidth); w++)
            {
                if (levelData[h][w] == "0")
                {
                    continue;
                }

                // TODO: Non-zero values will eventually have new distinction (different color bricks, points, etc)

                var position = new Vector3(w + Offset.x, Offset.y - h);
                Instantiate(BrickTemplate, position, Quaternion.identity);
                _brickCount++;

                if (_brickCount >= MaxBricks)
                {
                    break;
                }
            }

            if (_brickCount >= MaxBricks)
            {
                break;
            }
        }

        GameManager.Instance.SetGameState(GameState.WaitForStart);
    }

    private bool CheckIfValidLevelData(string[][] levelData)
    {
        if (levelData == null)
        {
            Debug.Log("Level data is null.");
            return false;
        }

        if (levelData.Length == 0)
        {
            Debug.Log("Level data (first array) is empty.");
            return false;
        }

        if (levelData.Length > MaxLevelHeight)
        {
            Debug.Log($"Level data is too large. Must be a max of {MaxLevelHeight} rows");
            return false;
        }

        for (var h = 0; h < levelData.Length; h++)
        {
            var levelRow = levelData[h];
            for (var w = 0; w < levelRow.Length; w++)
            {
                if (levelRow.Length == 0)
                {
                    Debug.Log($"Level data row {w} is empty.");
                }
                else if (levelRow.Length > MaxLevelWidth)
                {
                    Debug.Log($"Level data row {w} has {levelRow.Length} rows. Max is {MaxLevelWidth}");
                    return false;
                }
            }
        }

        if (levelData.Any(levelRow => levelRow.Length == 0))
        {
            Debug.Log("Level data has empty rows.");
            return false;
        }

        if (levelData.Any(levelRow => levelRow.Length > MaxLevelWidth))
        {
            Debug.Log($"Level data is too large. Must be a max of {MaxLevelWidth} columns");
            return false;
        }

        return true;
    }

    public void RemoveBricks(int count)
    {
        _brickCount -= count;
        if (_brickCount <= 0)
        {
            _brickCount = 0;
            GameManager.Instance.SetGameState(GameState.LevelComplete);
        }
    }
    
    private void LevelCompleted(GameState newState)
    {
        if (newState == GameState.LevelComplete)
        {
            _currentLevel++;
            if (_currentLevel > NumberOfLevels)
            {
                _currentLevel = NumberOfLevels;
            }

            LevelText.text = $"Level {_currentLevel}";
            BuildLevel(_currentLevel);
            // TODO: Load the next level
            // BUG: Will this race for condition in the other events?
        }
    }

    private string GetFileNameForLevel(int level)
    {
        const string fileBaseName = "level";
        return $"Levels/{fileBaseName}_{level}";
    }

    private string[][] GetLevel(int level)
    {
        var fullFileName = GetFileNameForLevel(level);
        var textAsset = Resources.Load<TextAsset>(fullFileName);
        if (textAsset == null)
        {
            return null;
        }

        var text = textAsset.text;
        var lines = Regex.Split(text, "\n");
        var rows = lines.Length;

        var levelBase = new string[rows][];
        for (var i = 0; i < lines.Length; i++)
        {
            var stringsOfLine = lines[i].Select(c => c.ToString()).ToArray();
            levelBase[i] = stringsOfLine;
        }

        return levelBase;
    }
}
