using UnityEngine;

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockBack;
    private SpriteRenderer spriteRenderer;
    private EnemyAi enemyAi;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAi = GetComponent<EnemyAi>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockBack = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (knockBack.GettingKnockedBack == true)
        {
            if (animator != null)
                animator.SetBool("Moving", false);
            return;
        }

        rb.linearVelocity = speed * moveDir;

        if(rb.linearVelocity == Vector2.zero)
        {
            if (animator != null)
                animator.SetBool("Moving", false);
        }
        else
        {
            if (animator != null)
                animator.SetBool("Moving", true);
        }

        if (moveDir.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveDir.x > 0)
        {
            spriteRenderer.flipX = true;
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
