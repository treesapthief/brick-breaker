using UnityEngine;

public class Ball : MonoBehaviour
{
    public float Speed = 30;
    public Vector2 StartingPosition;

    private Rigidbody2D _rigidBody2D;


    private void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        GameManager.Instance.OnStateChange += UpdateBallMovement;
    }

    private void Update()
    {
        CheckIfBallIsOutOfBounds();
    }

    private void CheckIfBallIsOutOfBounds()
    {
        var position = transform.position;
        var isOutOfBounds = (position.x < -25 || position.x > 25 || position.y < -16 || position.y > 16);
        if (isOutOfBounds)
        {
            GameManager.Instance.SetGameState(GameState.WaitForStart);
        }
    }

    private void Move(GameState state)
    {
        if (DisallowMovement(state))
        {
            _rigidBody2D.velocity = Vector2.zero;
        }
        else
        {
            _rigidBody2D.velocity = Vector2.up * Speed;
        }
    }

    private void ResetPosition()
    {
        _rigidBody2D.position = StartingPosition;
        _rigidBody2D.velocity = Vector2.zero;
    }

    private void UpdateBallMovement(GameState newState)
    {
        Move(newState);
        if (newState == GameState.NewGame || newState == GameState.WaitForStart)
        {
            ResetPosition();
        }
    }

    private static bool DisallowMovement(GameState gameState)
    {
        return gameState != GameState.InGame;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Racket")
        {
            var x = HitFactor(transform.position, collision.transform.position,
            collision.collider.bounds.size.x);
            var dir = new Vector2(x, 1).normalized;

            GetComponent<Rigidbody2D>().velocity = dir * Speed;
        }

        // TODO: Bounce factor on Block tiles?
        //if (collision.gameObject.tag == "Block")
        //{
        //    var y = HitFactor(transform.position,
        //    collision.transform.position,
        //    collision.collider.bounds.size.y);
        //    var dir = new Vector2(-1, y).normalized;
        //    GetComponent<Rigidbody2D>().velocity = dir * Speed;
        //}
    }

    private static float HitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    {
        return (ballPos.x - racketPos.x) / racketHeight;
    }
}
