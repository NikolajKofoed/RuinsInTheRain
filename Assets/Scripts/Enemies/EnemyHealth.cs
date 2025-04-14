using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyHealth : MonoBehaviour
{
	[field: SerializeField] public int maxHealth { get; set; } = 10;
	[SerializeField] private float knockbackForce = 15f;
	[SerializeField] private GameObject deathVfxPrefab;

	private int currentHealth;
	private Animator anim;
	private Knockback knockback;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		currentHealth = maxHealth;
		anim = GetComponent<Animator>();
		knockback = GetComponent<Knockback>();
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
        if (!other.CompareTag("PlayerProjectile"))
        {
            knockback.GetKnockedBack(other, knockbackForce);
        }
        //animator.SetTrigger("Hurt")

        CheckIfDead(other);
	}

    void CheckIfDead(Transform other)
    {
        if (currentHealth <= 0)
        {
            //animator.SetBool("IsDead", true);
            if (deathVfxPrefab != null)
            {
                PlayVFX(other);
            }
            GetComponent<PickupSpawner>()?.DropItems();
            gameObject.SetActive(false); // disable object
            //anim?.SetBool("Dies", true);
        }
    }



    private void PlayVFX(Transform other)
    {
        GameObject vfxInstance = Instantiate(deathVfxPrefab, transform.position, Quaternion.identity);

        Vector3 directionAwayFromPlayer = Vector3.right; // default direction

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

                // You can use separate X/Y curves or just set a constant
                vel.x = directionAwayFromPlayer.x * 5f;
                vel.y = directionAwayFromPlayer.y * 5f;
            }

            ps.Play();
        }

        // Clean up
        float maxDuration = 0f;
        foreach (var ps in vfxInstance.GetComponentsInChildren<ParticleSystem>())
        {
            if (ps.main.duration > maxDuration)
                maxDuration = ps.main.duration;
        }

        Destroy(vfxInstance, maxDuration + 1f);
    }


}
