using UnityEngine;

public class ShooterRobot : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject firePointLeft;
    [SerializeField] private GameObject firePointRight;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform shootingDir;
    private EnemyAudio audioSource;

    readonly int ATTACK_HASH = Animator.StringToHash("Shoot");

    private void Awake()
    {
        audioSource = GetComponent<EnemyAudio>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Attack()
    {
        animator.SetTrigger(ATTACK_HASH);

        if (transform.position.x - Player2D.Instance.transform.position.x < 0)
        {
            spriteRenderer.flipX = true;
            shootingDir = firePointRight.transform;
        }
        else
        {
            spriteRenderer.flipX = false;
            shootingDir = firePointLeft.transform;
        }
    }

    public void SpawnProjectileAnimEvent()
    {
        audioSource.PlayAttackSound();
        Instantiate(projectilePrefab, shootingDir.position, Quaternion.identity);
    }
}
