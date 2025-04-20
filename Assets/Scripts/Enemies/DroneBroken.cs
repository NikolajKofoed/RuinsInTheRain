using UnityEngine;

public class DroneBroken : MonoBehaviour, IEnemy
{
    // Movement
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 moveDirection = new Vector2(1f, 0.5f);
    [SerializeField] private LayerMask SurfaceLayer;
    [SerializeField] bool goingUp = true;

    private Rigidbody2D rb;
	private BoxCollider2D boxCollider;
	[field: SerializeField] private float damage;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HitLogic();
    }

	void FixedUpdate()
	{
		rb.MovePosition(rb.position + (moveDirection * Time.deltaTime));
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // When this enemy touches the player, the player takes damage
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }

	// What should happen when the drone hit something
	private void HitLogic()
    {
        if (TouchedGround() && !goingUp)
        {
            ChangeYDirection();
        }
        if (TouchedRoof() && goingUp)
        {
            ChangeYDirection();
        }
        if (TouchedWall())
        {
            Flip();
        }

    }

    // Detect if the drone hit a surface and from where
	private bool TouchedGround()
	{
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, SurfaceLayer);
		return raycastHit.collider != null;
	}

	private bool TouchedRoof()
	{
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, SurfaceLayer);
		return raycastHit.collider != null;
	}

	private bool TouchedWall()
	{
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, SurfaceLayer);
		return raycastHit.collider != null;
	}

    // Changes the drones direction
    private void ChangeYDirection()
    {
        moveDirection.y = -moveDirection.y;
        goingUp = !goingUp;
    }

    private void Flip()
    {
        transform.Rotate(new Vector2(0, 180));
        moveDirection.x = -moveDirection.x;
    }

    public void Attack()
    {

    }
}
