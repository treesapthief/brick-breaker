using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public delegate void OnLevelChangedHandler(int level);

public class LevelManager
{
    public event OnLevelChangedHandler OnLevelChanged;
    public Vector3 Offset = new Vector2(-23, 14);
    public int MaxLevelWidth = 48;
    public int MaxLevelHeight = 24;
    public int MaxBricks = 120;
    public int NumberOfLevels = 1;
    private int _brickCount = 0;
    private int _currentLevel = 1;

    private static LevelManager _instance = null;

    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LevelManager();
            }

            return _instance;
        }
    }

    public LevelManager()
    {
        GameManager.Instance.OnStateChange += LevelCompleted;
        //SetLevel(1);
    }

    private void ClearLevel()
    {
        Debug.Log("Clear existing bricks from level");
        var bricks = Object.FindObjectsOfType<Brick>();
        foreach (var brick in bricks)
        {
            Object.Destroy(brick.gameObject);
        }
    }

    public void BuildLevel(int level)
    {
        ClearLevel();
        Debug.Log($"Building Level {level}");
        _brickCount = 0;
        var levelData = GetLevel(level);
        
        if (!CheckIfValidLevelData(levelData))
        {
            Debug.Log("Invalid level data");
            return;
        }

        for (var h = 0; h < Math.Min(levelData.Length, MaxLevelHeight); h++) {
            for (var w = 0; w < Math.Min(levelData[h].Length, MaxLevelWidth); w++)
            {
                var tile = levelData[h][w];
                if (tile == "0")
                {
                    continue;
                }
                var hitPoints = 1;
                var color = Color.white;
                switch (tile)
                {
                    case "1":
                        hitPoints = 1;
                        break;
                    case "2":
                        hitPoints = 2;
                        color = Color.red;
                        break;
                    case "3":
                        hitPoints = 3;
                        color = Color.yellow;
                        break;
                    case "4":
                        hitPoints = 4;
                        color = Color.green;
                        break;
                }

                var position = new Vector3(w + Offset.x, Offset.y - h);
                var brickTemplate = Resources.Load("Prefabs/Brick");
                var brickObject = (GameObject)Object.Instantiate(brickTemplate, position, Quaternion.identity);
                var brick = brickObject.GetComponent<Brick>();
                if (brick != null)
                {
                    brick.InitializeHealth(hitPoints);
                    brick.SetColor(color);
                }

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
        Debug.Log("LevelManager.LevelCompleted");
        if (newState == GameState.LevelComplete)
        {
            SetLevel(_currentLevel + 1);
        }
        else if (newState == GameState.NewGame)
        {
            SetLevel(1);
        }
    }

    private static string GetFileNameForLevel(int level)
    {
        const string fileBaseName = "level";
        return $"Levels/{fileBaseName}_{level}";
    }

    private static string[][] GetLevel(int level)
    {
        var fullFileName = GetFileNameForLevel(level);
        var textAsset = Resources.Load<TextAsset>(fullFileName);
        if (textAsset == null)
        {
            return null;
        }

        var text = textAsset.text;
        var lines = text.Split(new [] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        var rows = lines.Length;

        var levelBase = new string[rows][];
        for (var i = 0; i < lines.Length; i++)
        {
            var stringsOfLine = lines[i].Select(c => c.ToString()).ToArray();
            levelBase[i] = stringsOfLine;
        }

        return levelBase;
    }

    private void SetLevel(int level)
    {
        _currentLevel = level;
        if (_currentLevel > NumberOfLevels)
        {
            _currentLevel = NumberOfLevels;
        }

        Debug.Log($"OnLevelChanged: {_currentLevel}");
        OnLevelChanged?.Invoke(_currentLevel);
        BuildLevel(_currentLevel);
    }
}
