using System.Collections;
using UnityEngine;

public class NewAbility : MonoBehaviour
{
	// Ability number
	[SerializeField] private int Ability;
	// Sound
	[SerializeField] private AudioClip pickupSound;
	private AudioSource audioSource;
	// Text for when unlocking new ability
	[SerializeField] public GameObject newAbilityText;
	[SerializeField] private float showAbilityTextTimer;

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
			StartCoroutine(ShowNewAbilityText());
			gameObject.SetActive(false);
		}
	}

	// Makes it so the text is only shown for a 'x' amount of time
	private IEnumerator ShowNewAbilityText()
	{
		newAbilityText.SetActive(true);
		yield return new WaitForSeconds(showAbilityTextTimer);
		Debug.Log("Deactivating ability text"); //Why does this not happen
		newAbilityText.SetActive(false);
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
