using UnityEngine;

public class Brick : MonoBehaviour
{
    public int Score = 100;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Ball")
        {
            ScoreManager.Instance.GivePoints(Score);
            LevelManager.Instance.RemoveBricks(1);
            Destroy(gameObject);
        }
    }
}
