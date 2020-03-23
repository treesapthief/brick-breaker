using UnityEngine;

public class MoveRacket : MonoBehaviour
{
    public float Speed = 30;
    public float QuickSpeedMultiplier = 2;
    public string Axis = "Horizontal";
    public string QuickSpeedButton = "QuickSpeed";
    public Vector2 StartingPosition;

    private Rigidbody2D _rigidBody2D;

    private void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        GameManager.Instance.OnStateChange += SetRacket;
        GameManager.Instance.OnRestartLevel += SetRacket;
    }

    private void SetRacket(GameState newState)
    {
        if (newState == GameState.NewGame)
        {
            SetRacket();
        }
    }

    private void SetRacket()
    {
        transform.position = StartingPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (DisallowMovement())
        {
            return;
        }

        var horizontalAxis = Input.GetAxisRaw(Axis);
        var quickSpeedEnabled = Input.GetButton(QuickSpeedButton);
        _rigidBody2D.velocity = new Vector2(horizontalAxis, 0) * Speed * (quickSpeedEnabled ? QuickSpeedMultiplier : 1);
    }

    private static bool DisallowMovement()
    {
        var gameState = GameManager.Instance.GameState;
        return gameState == GameState.GameOver || gameState == GameState.Paused;
    }
}