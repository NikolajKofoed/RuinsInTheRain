using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

// refactor update to make it look cleaner - nik

/// <summary>
/// The Player movement script
/// </summary>
public class Player2D : Singleton<Player2D>
{
	[Header("Moovement Paramaters")]
	[field: SerializeField] public float Speed { get; private set; } = 5.0f;
	[field: SerializeField] private float JumpPower { get; set; } = 10.0f;

	[Header("Multi jump")]
	[SerializeField] private int ExtraJumps;
	[SerializeField] private float JumpBuffer = 0.10f; // allows you to jump after not touching the ground for x amount of time
	private int JumpCounter; // For resetting ExtraJump
	private float airTime = 0;

	[Header("Wall Jumping")]
	[SerializeField] private bool canWallJump;
    [field: SerializeField] private float WallJumpX; //Horizontal wall jump force
    [field: SerializeField] private float WallJumpY; //Vertical wall jump force
	// maybe we can seperate groundlayer / walllayer on the tilemap by individual tiles?
    [Header("Layers")]
    [field: SerializeField] private LayerMask SurfaceLayer;

	[Header("Dash")]
	[SerializeField] private bool canDash;
	[field: SerializeField] private float DashLength;
	[field: SerializeField] private float DashDuration = 0.2f; //Durantion of the dash
	[field: SerializeField] private int DashMaxAirAmounts;
	private int DashCounter; // For resetting DashAmounts
	private float DashCooldown = 0.1f;

	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	private SpriteRenderer spriteRenderer;
	private Animator anim;
	private Knockback knockback;
	private PlayerAudio playerAudio;
	private float horizontalInput;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	protected override void Awake()
    {
		base.Awake();

		playerAudio = GetComponent<PlayerAudio>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		knockback = GetComponent<Knockback>();

		// This is to deleted the player character when the player return to the main menu
		DontDestroyOnLoad(gameObject);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

    private void Start()
    {
		canWallJump = PowerupManager.Instance.WallJumpUnlocked;
		canDash = PowerupManager.Instance.DashUnlocked;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "MainMenu")
		{
			Destroy(gameObject);
		}
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		
	}

	private void Update()
	{
		horizontalInput = Input.GetAxis("Horizontal");

		// FLip player when moving left
		if (horizontalInput > 0.01f)
		{
			transform.localScale = Vector3.one;
			//spriteRenderer.flipX = false; // It is this that break the wall Jump.
		}
		else if (horizontalInput < -0.01f)
		{
			transform.localScale = new Vector3(-1, 1, 1);
			//spriteRenderer.flipX = true;
		}

		//Set animaator parametors
		anim.SetBool("Walking", horizontalInput != 0);
		anim.SetBool("Grounded", IsGrounded());
		anim.SetBool("OnWall", OnWall());
		anim.SetBool("CanWallJump", canWallJump);

		//Jump
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}
		
		if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y / 2);
		}

		//Dash
		if (Input.GetKeyDown(KeyCode.LeftShift) && DashCooldown < 0 && DashCounter > 0) //Cooldown hasn't been made yet to work
		{
			//anim.SetTrigger("Dash");
			Dash();
		}

		if (OnWall() && !IsGrounded() && canWallJump)
		{
			rb.gravityScale = 1;
			rb.linearVelocity = Vector2.zero;
			JumpCounter = ExtraJumps; //Regain extra jump, while clining to wall
			DashCounter = DashMaxAirAmounts;
		}
		else if(!knockback.GettingKnockedBack)
		{
			rb.gravityScale = 2;
			rb.linearVelocity = new Vector2(horizontalInput * Speed, rb.linearVelocity.y);

			if (IsGrounded())
			{
				JumpCounter = ExtraJumps;
				DashCounter = DashMaxAirAmounts;
			}
		}

		if (IsGrounded() || (OnWall() && canWallJump)) 
		{
			airTime = 0f; // track airtime
        }
		else
		{
			airTime += Time.deltaTime; // if on ground reset
		}

		DashCooldown -= Time.deltaTime;
	}

	private void Jump()
	{
		//SoundManager.instance.PlaySound(jumpSound);
		if (OnWall() && !IsGrounded() && !knockback.GettingKnockedBack && canWallJump)
		{
			WallJump();
		}
		else if (!knockback.GettingKnockedBack)
        {
			if (IsGrounded() || (JumpBuffer >= airTime && !OnWall()))
			{
				playerAudio.PlayJump();
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpPower);
			}
			else
			{
				if (JumpCounter > 0) //If we have extra  jumps, then jump and decrease the jump counter
				{
					playerAudio.PlayAirJump();
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpPower);
					JumpCounter--;
				}
			}
		}
	}

	private void WallJump()
	{
		if (!knockback.GettingKnockedBack)
		{
            rb.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * WallJumpX, WallJumpY));
        }
    }

	private void Dash()
	{
		if(!canDash) { return; }

		if (!knockback.GettingKnockedBack && !OnWall() || canWallJump && !knockback.GettingKnockedBack)
		{
			DashCooldown = DashDuration + 0.2f; // Reset cooldown time after a dash
			StartCoroutine(PerformDash());
			DashCounter--;
		}

    }

	private IEnumerator PerformDash()
	{
		float originalGravity = rb.gravityScale;
		Vector2 originalVelocity = rb.linearVelocity;

		rb.gravityScale = 0f;
		rb.linearVelocity = Vector2.zero; // Stop vertical movement during dash

		// Determine dash direction
		float dashDirection = Mathf.Sign(transform.localScale.x);
		Vector2 startPosition = transform.position;
		Vector2 targetPosition = new Vector2(startPosition.x + dashDirection * DashLength, startPosition.y);

		float timeElapsed = 0f;
		float dashStep = 0.05f;  // Small step to check collisions frequently

		// Continue the dash while the time has not exceeded the dash duration
		while (timeElapsed < DashDuration)
		{
			// Calculate the new position based on the Lerp	
			Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, timeElapsed / DashDuration);

			// Perform a more granular collision check
			if (CheckDashCollision(newPosition))
			{
				// If a collision occurs, stop the dash immediately
				transform.position = newPosition;
				yield break; // Break out of the coroutine early
			}

			// If no collision, continue to move
			transform.position = newPosition;
			timeElapsed += Time.deltaTime; // Increment the time

			yield return null; // Wait for the next frame
		}

		// Ensure the final dash position is set
		transform.position = targetPosition;
		rb.gravityScale = originalGravity;
	}

	private bool CheckDashCollision(Vector2 targetPosition)
	{
		// BoxCast checks if a collision occurs along the player's dash path
		Vector2 direction = targetPosition - (Vector2)transform.position;

        Vector2 boxSize = boxCollider.size;
        boxSize.y *= 0.9f; // shrink box so character doesn't activate it by standing on the ground(meaning you can't dash when grounded)

        // Perform a BoxCast for collision detection
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0f, direction.normalized, direction.magnitude, SurfaceLayer);

		// Return true if a collision happens
		return hit.collider != null;
	}


    private bool IsGrounded()
	{
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, SurfaceLayer);
		return raycastHit.collider != null;
	}

	private bool OnWall()
	{
        Vector2 boxSize = boxCollider.size;
        boxSize.y *= 0.5f; 

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxSize, 0, new Vector2(transform.localScale.x, 0), 0.1f, SurfaceLayer);
		return raycastHit.collider != null;
	}

	public bool CanAttack()
	{
		return true;
	}

	public void UnlockAbility(int abilityNr)
	{
		if (abilityNr == 1)
		{
			canWallJump = true;
			PowerupManager.Instance.WallJumpUnlocked = true;
		}
		if (abilityNr == 2)
		{
			canDash = true;
            PowerupManager.Instance.DashUnlocked = true;
        }
    }
}