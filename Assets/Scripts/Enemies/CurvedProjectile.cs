using System.Collections;
using UnityEngine;

public class CurvedProjectile : MonoBehaviour
{
    private float arcHeight = 3f;
    private float gravityScale = 1f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        LaunchToTarget(Player2D.Instance.transform.position);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>()?.TakeDamage(1, transform);
        }

        Destroy(gameObject);
    }

    public void LaunchToTarget(Vector3 targetPosition)
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = targetPosition;

        Physics2D.gravity = new Vector2(0, -9.81f); // Ensure standard gravity
        rb.gravityScale = gravityScale;

        Vector2 velocity = CalculateArcVelocity(startPos, endPos, arcHeight, Physics2D.gravity.y * gravityScale);
        rb.linearVelocity = velocity;
    }

    private Vector2 CalculateArcVelocity(Vector2 start, Vector2 end, float height, float gravity)
    {
        float displacementY = end.y - start.y;
        Vector2 displacementXZ = new Vector2(end.x - start.x, 0f);

        // Calculate time to reach the apex
        float timeToApex = Mathf.Sqrt(2 * height / -gravity);
        float totalTimeDown = Mathf.Sqrt(2 * (height - displacementY) / -gravity);
        float totalTime = timeToApex + totalTimeDown;

        // Horizontal velocity
        float vx = displacementXZ.x / totalTime;

        // Initial vertical velocity to reach desired height
        float vy = Mathf.Sqrt(-2 * gravity * height);

        return new Vector2(vx, vy);
    }


}



