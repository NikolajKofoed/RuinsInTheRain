using UnityEngine;

public class ShooterRobot : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject firePointLeft;
    [SerializeField] private GameObject firePointRight;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform shootingDir;

    readonly int ATTACK_HASH = Animator.StringToHash("Shoot");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Attack()
    {
        animator.SetTrigger(ATTACK_HASH);

        if (transform.position.x - Player2D.Instance.transform.position.x < 0)
        {
            spriteRenderer.flipX = false;
            shootingDir = firePointRight.transform;
        }
        else
        {
            spriteRenderer.flipX = true;
            shootingDir = firePointLeft.transform;
        }
    }

    public void SpawnProjectileAnimEvent()
    {
        
        Instantiate(projectilePrefab, shootingDir.position, Quaternion.identity);
    }
}
