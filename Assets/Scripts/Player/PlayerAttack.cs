using System.Collections;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    // Melee attacks
    [SerializeField] private float meleeCooldown;
    [SerializeField] Transform meleePoint;
    [SerializeField] private float meleeRange;
    [SerializeField] private int MeleeDamage;
    [SerializeField] LayerMask enemyLayers;

    // Range attacks
    [SerializeField] private float projectileCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] public GameObject playerProjectiles { get; private set; }

    const string PROJECTILE_HOLDER = "ProjectileHolder";

    private Animator anim;
    private Player2D playerMovement;
    private float meleeCooldownTimer = Mathf.Infinity;
    private float rangedCooldownTimer = Mathf.Infinity;
    private bool canMeleeAttack = true;

    private List<Projectile> projectilePool = new List<Projectile>();

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<Player2D>();
    }

    private void Start()
    {
        if (playerProjectiles == null)
        {
            playerProjectiles = GameObject.Find(PROJECTILE_HOLDER);
        }
        // Manually cache inactive projectiles from the pool
        foreach (Transform child in playerProjectiles.transform)
        {
            Projectile p = child.GetComponent<Projectile>();
            if (p != null)
            {
                projectilePool.Add(p);
            }
        }

        Debug.Log("Cached " + projectilePool.Count + " projectiles.");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.J) && meleeCooldownTimer > meleeCooldown && playerMovement.CanAttack())
        {
            MeleeAttack();
        }

        if (Input.GetKey(KeyCode.K) && rangedCooldownTimer > projectileCooldown && playerMovement.CanAttack())
        {
            RangedAttack();
        }

        meleeCooldownTimer += Time.deltaTime;
        rangedCooldownTimer += Time.deltaTime;
    }

    public void MeleeAttack()
    {
        if (!canMeleeAttack) { return; }

        canMeleeAttack = false;
        anim.SetTrigger("MeleeAttack");
        meleeCooldownTimer = 0;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, meleeRange, enemyLayers);

        Debug.Log("Melee attack occurred");
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(MeleeDamage, this.transform);
            Debug.Log($"Hit enemy: " + enemy);
        }

        StartCoroutine(MeleeAttackCooldownRoutine());
    }

    private IEnumerator MeleeAttackCooldownRoutine()
    {
        yield return new WaitForSeconds(meleeCooldown);
        canMeleeAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        if (meleePoint == null) return;
        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
    }

    private void RangedAttack()
    {
        Debug.Log("Ranged attack occurred");
        anim.SetTrigger("RangeAttack");
        rangedCooldownTimer = 0;

        bool fired = false;

        foreach (var projectile in projectilePool)
        {
            if (!projectile.isActiveAndEnabled)
            {
                projectile.gameObject.SetActive(true);
                projectile.transform.position = firePoint.position;
                projectile.SetDirection(Mathf.Sign(transform.localScale.x));
                fired = true;
                break;
            }
        }

        if (!fired)
        {
            Debug.LogWarning("No inactive projectiles available in pool!");
        }
    }
}