using UnityEngine;

public class Bounds : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var gameManager = GameManager.Instance;
        if (gameManager.GameState != GameState.InGame)
        {
            return;
        }

        if (collision.collider.gameObject.name == "Ball")
        {
            Debug.Log("Point Scored");
            gameManager.LivesManager.TakeLives(1);
            if (gameManager.GameState == GameState.InGame)
            {
                gameManager.SetGameState(GameState.WaitForStart);
            }
        }
    }
}
