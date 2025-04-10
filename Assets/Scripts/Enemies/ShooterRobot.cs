using UnityEngine;

public class ShooterRobot : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject ProjectilePrefab;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Attack()
    {
    }

    public void SpawnProjectileAnimEvent()
    {
        Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
    }
}
