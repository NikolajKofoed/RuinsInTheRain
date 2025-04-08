using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	[field: SerializeField] public int maxHealth { get; set; } = 10;
	[SerializeField] private int Damage = 1;
	private int currentHealth;
	private Animator anim;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		currentHealth = maxHealth;
		anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
			var playerHealth = collision.GetComponent<Health>();

			playerHealth.TakeDamage(Attack());
        }
    }

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

	public int Attack()
	{
		return Damage;
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
