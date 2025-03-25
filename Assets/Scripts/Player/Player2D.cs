using UnityEngine;

// refactor update to make it look cleaner - nik

/// <summary>
/// The Player movement script
/// </summary>
public class Player2D : Singleton<Player2D>
{
	[Header("Moovement Paramaters")]
	[field: SerializeField] private float speed { get; set; } = 5.0f;
	[field: SerializeField] private float jumpPower { get; set; } = 10.0f;

	[Header("Multi jump")]
	[SerializeField] private int extraJumps;
	private int jumpCounter;

	[Header("Wall Jumping")]
    [field: SerializeField] private float wallJumpX; //Horizontal wall jump force
    [field: SerializeField] private float wallJumpY; //Vertical wall jump force

	// maybe we can seperate groundlayer / walllayer on the tilemap by individual tiles?
    [Header("Layers")]
    [field: SerializeField] private LayerMask groundLayer;
	[field: SerializeField] private LayerMask wallLayer;

	private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	private SpriteRenderer spriteRenderer;
	private Animator anim;
	//private float wallJumpCooldown;
	private float horizontalInput;

    protected override void Awake()
    {
		base.Awake();

        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

	}

	// Update is called once per frame
	void Update()
	{
		horizontalInput = Input.GetAxis("Horizontal");

		// FLip player when moving left
		// i've made it use the sprite renderers flipx property instead - nik
		if (horizontalInput > 0.01f)
		{
			//transform.localScale = Vector3.one;

			spriteRenderer.flipX = false;
		} else if (horizontalInput < -0.01f)
		{
			//transform.localScale = new Vector3(-1, 1, 1);
			spriteRenderer.flipX = true;
		}

		//Set animaator parameters
		anim.SetBool("Walking", horizontalInput != 0);
		anim.SetBool("Grounded", isGrounded());

		if (Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}

		//Jump
		if (Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0)
		{
			rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y / 2);
		}

		if (OnWall())
		{
			rb.gravityScale = 1;
			rb.linearVelocity = Vector2.zero;
            jumpCounter = extraJumps; //Regain extra jump, while clining to wall
			Debug.Log("hey there");
        }
		else
		{
			rb.gravityScale = 2;
			rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);

			if (isGrounded())
			{
				jumpCounter = extraJumps;
			}
		}
	}

	private void Jump()
	{
		//SoundManager.instance.PlaySound(jumpSound);
		if (OnWall())
		{
			WallJump();
			Debug.Log("is on wall now");
		}
		else
		{
            if (isGrounded())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            }
            else
            {
                if (jumpCounter > 0) //If we have extra  jumps, then jump and decrease the jump counter
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                    jumpCounter--;
                }
            }
        }
	}

	private void WallJump()
	{
        rb.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        //wallJumpCooldown = 0;
    }

	private bool isGrounded()
	{
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
		return raycastHit.collider != null;
	}

	private bool OnWall()
	{
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
		return raycastHit.collider != null;
		
	}

	public bool canAttack()
	{
		return true;
	}
}