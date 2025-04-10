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

    //private Animator anim;
    private Player2D playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        //anim = GetComponent<Animator>();
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

    private void MeleeAttack()
    {
        //anim.SetTrigger("meleeAttack")
        cooldownTimer = 0;
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, meleeRange, enemyLayers);
        Debug.Log("Melee attack occured");
        // Damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(MeleeDamage, this.transform);
            Debug.Log($"hit enemy: " + enemy);
        }
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
        //anim.SetTrigger("rangeAttack");
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
