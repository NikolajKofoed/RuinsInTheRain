using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Processors;

public class Health : MonoBehaviour
{
	[Header("Health")]
	[SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
	private Animator anim;

	// Respawn
	private Vector3 respawnPoint;

	[Header("iFrames")]
	[SerializeField] private float iFramesDuration;
	[SerializeField] private int numberOfFlashes;
	private SpriteRenderer spriteRend;

	[Header("sfx")]
	[field: SerializeField] public UnityEvent OnHeal { get; set; }
	[field: SerializeField] public UnityEvent OnHitByEnemy { get; set; }
	[field: SerializeField] public UnityEvent OnRespawnHazard { get; set; }
	[field: SerializeField] public UnityEvent OnDie { get; set; }

	[SerializeField] private float knockbackForce = 15f;
	private Knockback knockback;


	private void Awake()
	{
		currentHealth = startingHealth;
		anim = GetComponent<Animator>();
		spriteRend = GetComponent<SpriteRenderer>();
		knockback = GetComponent<Knockback>();
		respawnPoint = transform.position;
	}

    // Sets the player respawn point, when they touch a checkpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
    }

    /// <summary>
    /// Takes damage and gets knocked back
    /// </summary>
    /// <param name="_damage">amount of damage</param>
    /// <param name="otherTransform">damage dealers transform</param>
    public void TakeDamage(float _damage, Transform otherTransform)
	{
		if(_damage <= 0) { return; } // 0 or negative damage doesn't count as an attack

		if (currentHealth > 0 && knockback.GettingKnockedBack == false) // can't take damage while knockbacked
        {
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
            knockback.GetKnockedBack(otherTransform, knockbackForce);

            //anim.SetTrigger("hurt");
            OnHitByEnemy?.Invoke();
			//StartCoroutine(InvunerabilityRoutine()); // currently bugged

			CheckIfDead();
		}

		Debug.Log(currentHealth);
	}

	/// <summary>
	/// takes damage
	/// </summary>
	/// <param name="_damage">amount of damage</param>
	public void TakeDamage(float _damage)
	{
        if (_damage <= 0) { return; } // 0 or negative damage doesn't count as an attack

        if (currentHealth > 0 && knockback.GettingKnockedBack == false) // can't take damage while knockbacked
        {
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
            Debug.Log("Player took damage, current health: " + currentHealth);
            //anim.SetTrigger("hurt");
            OnHitByEnemy?.Invoke();
            //StartCoroutine(InvunerabilityRoutine());
			CheckIfDead();
        }
    }

	// Sets the player respawn point, when they touch a checkpoint
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Respawn")
		{
			respawnPoint = transform.position;
		}
	}
	// For when player comes into contact with extreme enviromental hazards
	public void RespawnHazard(float _hazardDamage)
	{
		currentHealth = Mathf.Clamp(currentHealth - _hazardDamage, 0, startingHealth);

		if (currentHealth > 0)
		{
			//anim.SetTrigger("hurt");
			OnRespawnHazard?.Invoke();
			//anim.ResetTrigger("");
			//anim.Play("idle");
			transform.position = respawnPoint;

			CheckIfDead();

            //StartCoroutine(InvunerabilityRoutine());

        }
    }

	public void AddHealth(float _heal)
	{
		OnHeal?.Invoke();
		currentHealth = Mathf.Clamp(currentHealth + _heal, 0, startingHealth);
	}

    private void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Plauer died");
            gameObject.SetActive(false); // disable object
            //anim?.SetBool("Dies", true);
        }
    }

    private IEnumerator InvunerabilityRoutine()
	{
		Physics2D.IgnoreLayerCollision(10, 11, true);
		for (int i = 0; i < numberOfFlashes; i++)
		{
			spriteRend.color = new Color(1, 0, 0, 0.0f);
			yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes));
			spriteRend.color = Color.white;
			yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes));
		}
		// invunerability duration
		Physics2D.IgnoreLayerCollision(10, 11, false);
	}
}