using UnityEngine;

public class ExtremeHazard : MonoBehaviour
{
	private Rigidbody2D rb;
	[field: SerializeField] private float damage;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		rb = GetComponent<Rigidbody2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// To deal damage to the player and make them respawn at last checkpoint.
		if (collision.gameObject.tag == "Player")
		{
			collision.GetComponent<Health>().RespawnHazard(damage);
		}
	}
}
