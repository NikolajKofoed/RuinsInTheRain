using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	[field: SerializeField] public int maxHealth { get; set; } = 10;
	[SerializeField] private float knockbackForce = 15f;
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
    public void TakeDamage(int damage, Transform other)
	{
		currentHealth -= damage;
		Debug.Log(currentHealth);
		knockback.GetKnockedBack(other, knockbackForce);
		//animator.SetTrigger("Hurt")

		if (currentHealth <= 0)
		{
			Dies();
		}
	}

	/// <summary>
	/// used for attacks where no knockback occurs
	/// </summary>
	/// <param name="damage">the amount of damage</param>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        //animator.SetTrigger("Hurt")

        if (currentHealth <= 0)
        {
            Dies();
        }
    }

    void Dies()
	{
		Debug.Log("Enemy died");
		//animator.SetBool("IsDead", true);

		GetComponent<Collider2D>().enabled = false;
		anim.SetBool("Dies", true);
		this.enabled = false;
	}
}
