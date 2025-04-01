using System;
using UnityEngine;

// refactor update to make it look cleaner - nik

/// <summary>
/// The Player movement script
/// </summary>
public class Player2D : Singleton<Player2D>
{
	[Header("Moovement Paramaters")]
	[field: SerializeField] private float Speed { get; set; } = 5.0f;
	[field: SerializeField] private float JumpPower { get; set; } = 10.0f;

	[Header("Multi jump")]
	[SerializeField] private int ExtraJumps;
	private int JumpCounter;

	[Header("Wall Jumping")]
    [field: SerializeField] private float WallJumpX; //Horizontal wall jump force
    [field: SerializeField] private float WallJumpY; //Vertical wall jump force
	// maybe we can seperate groundlayer / walllayer on the tilemap by individual tiles?
    [Header("Layers")]
    [field: SerializeField] private LayerMask SurfaceLayer;

	[Header("Dash")]
	[field: SerializeField] private float DashLength;
	private float DashCooldown = 0.1f;

	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	private SpriteRenderer spriteRenderer;
	private Animator anim;
	private float horizontalInput;


	// Start is called once before the first execution of Update after the MonoBehaviour is created
	protected override void Awake()
    {
		base.Awake();

        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}


	// Update is called once per frame
	void Update()
	{
		horizontalInput = Input.GetAxis("Horizontal");

		// FLip player when moving left
		if (horizontalInput > 0.01f)
		{
			transform.localScale = Vector3.one;
			// spriteRenderer.flipX = false; // It is this that break the wall Jump.
		}
		else if (horizontalInput < -0.01f)
		{
			transform.localScale = new Vector3(-1, 1, 1);
			//spriteRenderer.flipX = true;
		}

		//Set animaator parametors
		anim.SetBool("Walking", horizontalInput != 0);
		anim.SetBool("Grounded", IsGrounded());

		if (Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}

		//Jump
		if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y / 2);
		}

		//Dash
		if (Input.GetKeyDown(KeyCode.LeftShift) && DashCooldown > 1.0f) //Cooldown hasn't been made yet to work
		{
			Dash();
		}

		if (OnWall())
		{
			rb.gravityScale = 1;
			rb.linearVelocity = Vector2.zero;
			JumpCounter = ExtraJumps; //Regain extra jump, while clining to wall
		}
		else
		{
			rb.gravityScale = 2;
			rb.linearVelocity = new Vector2(horizontalInput * Speed, rb.linearVelocity.y);

			if (IsGrounded())
			{
				JumpCounter = ExtraJumps;
			}
		}
	}

	private void Jump()
	{
		//SoundManager.instance.PlaySound(jumpSound);
		if (OnWall())
		{
			WallJump();
		}
		else
		{
			if (IsGrounded())
			{
				rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpPower);
			}
			else
			{
				if (JumpCounter > 0) //If we have extra  jumps, then jump and decrease the jump counter
				{
					rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpPower);
					JumpCounter--;
				}
			}
		}
	}

	private void WallJump()
	{
		rb.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * WallJumpX, WallJumpY));
		//wallJumpCooldown = 0;
	}

	private void Dash()
	{
		rb.linearVelocity = new Vector2(DashLength, rb.linearVelocity.y); // Need to checked later if correct
	}

	private bool IsGrounded()
	{
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, SurfaceLayer);
		return raycastHit.collider != null;
	}

	private bool OnWall()
	{
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, SurfaceLayer);
		return raycastHit.collider != null;
	}

	public bool CanAttack()
	{
		return true;
	}
}