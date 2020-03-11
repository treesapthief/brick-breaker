using UnityEngine;

public class Brick : MonoBehaviour
{
    public float Score = 100;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "Ball")
        {
            Destroy(gameObject);
        }
    }
}
