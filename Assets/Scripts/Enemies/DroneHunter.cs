using UnityEngine;

public class DroneHunter : MonoBehaviour
{
	// Movement
	[SerializeField] private float speed;
	[SerializeField] private float lineOfSight;
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

    // So we can see what the enemy can see
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
	}
}
