using System.Collections;
using UnityEngine;

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
    [SerializeField] private GameObject[] playerProjectiles;

    private Animator anim;
    private Player2D playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private bool canMeleeAttack = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<Player2D>();
    }

    private void Update()
    {
		if (Input.GetKey(KeyCode.J) && cooldownTimer > meleeCooldown && playerMovement.CanAttack())
		{
			MeleeAttack();
		}
		if (Input.GetKey(KeyCode.K) && cooldownTimer > projectileCooldown && playerMovement.CanAttack())
        {
            RangedAttack();
        }
        cooldownTimer += Time.deltaTime;
    }

    public void MeleeAttack()
    {
        if (!canMeleeAttack) { return; } // can't attack if on cooldown
        canMeleeAttack = false;

        anim.SetTrigger("MeleeAttack");
        cooldownTimer = 0;

        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, meleeRange, enemyLayers);

        Debug.Log("Melee attack occured");
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(MeleeDamage, this.transform);
            Debug.Log($"hit enemy: " + enemy);
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
        if (meleePoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
    }

    private void RangedAttack()
    {
        anim.SetTrigger("RangeAttack");
        cooldownTimer = 0;

        playerProjectiles[FindProjectile()].transform.position = firePoint.position;
        playerProjectiles[FindProjectile()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }


    private int FindProjectile()
    {
        for (int i = 0; i < playerProjectiles.Length; i++)
        {
            if (!playerProjectiles[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
