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
	private bool isDead = false;
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


	private void Awake()
	{
		currentHealth = startingHealth;
		anim = GetComponent<Animator>();
		spriteRend = GetComponent<SpriteRenderer>();
		respawnPoint = transform.position;
	}

	public void TakeDamage(float _damage)
	{
		currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

		if (currentHealth > 0)
		{
			Debug.Log(currentHealth);
			//anim.SetTrigger("hurt");
			OnHitByEnemy?.Invoke();
			StartCoroutine(Invunerability());
		}
		else
		{
			if (!isDead)
			{
				OnDie?.Invoke();
				GetComponent<Player2D>().enabled = false; //ChangeName Depending on Player controller
				//anim.SetTrigger("die");
				isDead = true;
			}
		}
	}

	// Sets the player respawn point, when they touch a checkpoint
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Checkpoint")
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
			StartCoroutine(InvunerabilityRoutine());
		}
		else
		{
			if (!isDead)
			{
				OnDie?.Invoke();
				GetComponent<Player2D>().enabled = false; //ChangeName Depending on Player controller
														  //anim.SetTrigger("die");
				isDead = true;
			}
		}
	}

	public void AddHealth(float _heal)
	{
		OnHeal?.Invoke();
		currentHealth = Mathf.Clamp(currentHealth + _heal, 0, startingHealth);
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