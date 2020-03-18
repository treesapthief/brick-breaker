using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int Score = 100;
    public int HitPoints = 1;
    public int MaxHitPoints = 1;
    private SpriteRenderer _spriteRenderer;
    private Color _color = Color.white;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = _color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Ball")
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        var gameManager = GameManager.Instance;
        gameManager.ScoreManager.GivePoints(Score);
        HitPoints--;

        if (HitPoints <= 0)
        {
            HitPoints = 0;
            DestroyBrick();
        }
    }

    internal void DestroyBrick()
    {
        var gameManager = GameManager.Instance;
        gameManager.LevelManager.RemoveBricks(1);
        Destroy(gameObject);
    }

    internal void InitializeHealth(int hitPoints)
    {
        HitPoints = hitPoints;
        MaxHitPoints = hitPoints;
    }

    internal void SetColor(Color color)
    {
        _color = color;
    }
}
