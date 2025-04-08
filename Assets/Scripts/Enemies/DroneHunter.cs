using UnityEngine;

public class DroneHunter : MonoBehaviour, IEnemy
{
	// Movement
	[SerializeField] private float speed;
	[SerializeField] private float lineOfSight;
	[SerializeField] private int AttackDamage = 1;
	private bool hunterMode = false;

    private Rigidbody2D rb;
    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
	{
		float distantanceFromPlayer = Vector2.Distance(player.position, transform.position);
		if (distantanceFromPlayer < lineOfSight)
		{
			hunterMode = true;
		}
	}

    private void FixedUpdate()
	{
		if (hunterMode)
		{
			transform.position = Vector2.MoveTowards(rb.position, player.position, (speed * Time.deltaTime));
		}
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.CompareTag("Player"))
		{
			var playerHealth = collision.GetComponent<Health>();
			playerHealth?.TakeDamage(AttackDamage, this.transform);
		}
    }

    public int Attack()
	{
		return AttackDamage;
	}

    // So we can see what the enemy can see
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
	}
}
