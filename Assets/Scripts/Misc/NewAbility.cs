using UnityEngine;

public class NewAbility : MonoBehaviour
{
	[SerializeField] private int Ability;
	private Transform player;

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			collision.GetComponent<Player2D>().UnlockAbility(Ability); // Check Player2D scripts Unlock Ability to see what nr (abilty) you want to unlock
			gameObject.SetActive(false);
		}
	}
}
