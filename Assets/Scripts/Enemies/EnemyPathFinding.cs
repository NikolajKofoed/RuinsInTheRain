using UnityEngine;

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockBack;
    private SpriteRenderer spriteRenderer;
    private EnemyAi enemyAi;

    private void Awake()
    {
        enemyAi = GetComponent<EnemyAi>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockBack = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (knockBack.GettingKnockedBack == true) { return; }
        rb.linearVelocity = speed * moveDir;

        if (moveDir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDir.x > 0)
        {
            spriteRenderer.flipX = false;
        }

    }

    public void MoveTo(Vector2 targetPos)
    {
        moveDir = targetPos;
    }

    public void StopMoving()
    {
        moveDir = Vector3.zero;
    }
}
