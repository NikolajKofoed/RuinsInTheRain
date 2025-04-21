using UnityEngine;

public class NewAbility : MonoBehaviour
{
	// Ability number
	[SerializeField] private int Ability;
	// Sound
	[SerializeField] private AudioClip pickupSound;
	private AudioSource audioSource;

	private Transform player;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			PickUpSound();
			collision.GetComponent<Player2D>().UnlockAbility(Ability); // Check Player2D scripts Unlock Ability to see what nr (abilty) you want to unlock
			gameObject.SetActive(false);
		}
	}

	private void PickUpSound()
	{
		if (pickupSound == null || audioSource == null) return;

		GameObject tempGO = new GameObject("TempAudio");
		AudioSource tempSource = tempGO.AddComponent<AudioSource>();

		tempSource.clip = pickupSound;
		tempSource.volume = audioSource.volume;
		tempSource.pitch = audioSource.pitch;
		tempSource.spatialBlend = audioSource.spatialBlend;
		tempSource.Play();

		Destroy(tempGO, pickupSound.length);
	}
}
