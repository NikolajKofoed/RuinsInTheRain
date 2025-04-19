using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
	[Header("Health")]
	[SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
	private Animator anim;

    // Respawn
    private Vector3 respawnPoint;

	private Image totalHealthBar;
	private Image currentHealthBar;
	const string TOTAL_HEALTH_BAR = "HealthbarFull";
	const string CURRENT_HEALTH_BAR = "HealthbarCurrent";
    public GameOverMenu gameOver;

	[Header("iFrames")]
	[SerializeField] private float iFramesDuration;
	[SerializeField] private int numberOfFlashes;
	private SpriteRenderer spriteRend;

	[SerializeField] private float knockbackForce = 15f;
	private Knockback knockback;


	private void Awake()
	{
		
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
        if (collision.tag == "Respawn")
        {
            respawnPoint = transform.position;
        }
    }

    private void Start()
    {
		ResetHealth();
		respawnPoint = transform.position;
		UpdateHealthBar();
    }

	void ResetHealth()
	{
		currentHealth = startingHealth;

	}

	/// <summary>
	/// Takes damage and gets knocked back
	/// </summary>
	/// <param name="_damage">amount of damage</param>
	/// <param name="otherTransform">damage dealers transform</param>
	public void TakeDamage(float _damage, Transform otherTransform = null)
	{
		if(_damage <= 0) { return; } // 0 or negative damage doesn't count as an attack

		if (currentHealth > 0 && knockback.GettingKnockedBack == false) // can't take damage while knockbacked
        {
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
            UpdateHealthBar();

            if (otherTransform != null)
            {
                knockback.GetKnockedBack(otherTransform, knockbackForce);

            }

            //anim.SetTrigger("hurt");
            //StartCoroutine(InvunerabilityRoutine()); // currently bugged      
			CheckIfDead();
		}

		Debug.Log(currentHealth);
	}

	// For when player comes into contact with extreme enviromental hazards
	public void RespawnHazard(float _hazardDamage)
	{

		if (currentHealth > 0)
		{
            TakeDamage(_hazardDamage);

            //anim.SetTrigger("hurt");
			//anim.ResetTrigger("");
			transform.position = respawnPoint;
        }
    }

	public void AddHealth(float _heal)
	{
		currentHealth = Mathf.Clamp(currentHealth + _heal, 0, startingHealth);
		UpdateHealthBar();
	}

    private void CheckIfDead()
    {
        if (currentHealth <= 0)
        {
			currentHealth = 0;
			Debug.Log("Plauer died");
            //anim?.SetBool("Dies", true);
            //StartCoroutine(DeathLoadSceneRoutine());
            gameOver.GameOverUI();
            Destroy(gameObject);
		}
    }

    private void UpdateHealthBar()
    {
        if (totalHealthBar == null)
        {
            var totalObj = GameObject.Find(TOTAL_HEALTH_BAR);
            if (totalObj != null)
            {
                totalHealthBar = totalObj.GetComponent<Image>();
                Debug.Log("Found total healthbar: " + totalHealthBar.name);
            }
            else
            {
                Debug.LogWarning("Total health bar not found!");
            }
        }

        if (currentHealthBar == null)
        {
            var currentObj = GameObject.Find(CURRENT_HEALTH_BAR);
            if (currentObj != null)
            {
                currentHealthBar = currentObj.GetComponent<Image>();
                Debug.Log("Found current healthbar: " + currentHealthBar.name);
            }
            else
            {
                Debug.LogWarning("Current health bar not found!");
            }
        }

        if (totalHealthBar != null)
        {
            totalHealthBar.fillAmount = startingHealth;
        }

        if (currentHealthBar != null && startingHealth > 0)
        {
            currentHealthBar.fillAmount = currentHealth / startingHealth;
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