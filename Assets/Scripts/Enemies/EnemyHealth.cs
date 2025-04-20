using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyHealth : MonoBehaviour
{
    [field: SerializeField] public int maxHealth { get; set; } = 10;
    [SerializeField] private float knockbackForce = 15f;
    [SerializeField] private GameObject deathVfxPrefab;

    private int currentHealth;
    public int CurrentHealth => currentHealth;
    private Animator anim;
    private Knockback knockback;
    private EnemyAudio enemyAudio; // added
    private FlashOnDamage flash;

    private void Awake()
    {
        flash = GetComponent<FlashOnDamage>();
        anim = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
        enemyAudio = GetComponent<EnemyAudio>(); // cache audio component
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// used for attacks that have knockback
    /// </summary>
    /// <param name="damage">the amount of damage</param>
    /// <param name="other">position of the damage dealer</param>
    public void TakeDamage(int damage, Transform other = null)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        bool isDead = currentHealth <= 0;

        if (!other.CompareTag("PlayerProjectile"))
        {
            knockback.GetKnockedBack(other, knockbackForce);
        }

        if (isDead)
        {
            CheckIfDead(other);
        }
        else
        {
            //Only play hit sound if not dying
            enemyAudio?.PlayHitSound();

            if(flash != null)
            {
                StartCoroutine(flash.FlashRoutine());

            }
        }
    }


    void CheckIfDead(Transform other)
    {
        if (currentHealth <= 0)
        {
            var enemy = GetComponent<IEnemy>();
            if (enemy != null)
            {
                Player2D.Instance.gameObject.GetComponent<Health>().AddHealth(1);
            }

            // Play death sound (detached so it persists)
            enemyAudio?.PlayDeathSoundDetached();

            if (deathVfxPrefab != null)
            {
                PlayVFX(other);
            }

            GetComponent<PickupSpawner>()?.DropItems();

            gameObject.SetActive(false); // disable object
        }
    }

    private void PlayVFX(Transform other)
    {
        GameObject vfxInstance = Instantiate(deathVfxPrefab, transform.position, Quaternion.identity);

        Vector3 directionAwayFromPlayer = Vector3.right;

        if (other != null)
        {
            directionAwayFromPlayer = (transform.position - other.position).normalized;
            Debug.Log("Calculated direction: " + directionAwayFromPlayer);
        }

        foreach (var ps in vfxInstance.GetComponentsInChildren<ParticleSystem>())
        {
            if (ps.CompareTag("DynamicParticleSystem"))
            {
                var vel = ps.velocityOverLifetime;
                vel.enabled = true;
                vel.space = ParticleSystemSimulationSpace.Local;

                vel.x = directionAwayFromPlayer.x * 5f;
                vel.y = directionAwayFromPlayer.y * 5f;
            }

            ps.Play();
        }

        float maxDuration = 0f;
        foreach (var ps in vfxInstance.GetComponentsInChildren<ParticleSystem>())
        {
            if (ps.main.duration > maxDuration)
                maxDuration = ps.main.duration;
        }

        Destroy(vfxInstance, maxDuration + 1f);
    }
}