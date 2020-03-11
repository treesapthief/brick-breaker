using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject BrickTemplate;
    public Vector3 Offset;
    public int LevelWidth = 45;
    public int LevelHeight = 24;
    public int BrickWidth = 4;
    public int BrickHeight = 2;
    private static LevelManager _instance = null;

    public LevelManager()
    {

    }

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

    public void BuildLevel()
    {
        for (var w = 0; w < LevelWidth; w+=BrickWidth) {
            for (var h = 0; h < LevelHeight; h+=BrickHeight)
            {
                var position = new Vector3(w + Offset.x, h + Offset.y);
                Instantiate(BrickTemplate, position, Quaternion.identity);
            }
        }
    }
}
