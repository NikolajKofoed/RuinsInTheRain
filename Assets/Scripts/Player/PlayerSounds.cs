using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private AudioSource AudioSource;

    public AudioClip AudioclipHealthPickup;
    public AudioClip AudioclipDamageFromMonster;
	public AudioClip AudioclipRespawnHazard;
    public AudioClip AudioclipDie;


	private void Awake()
	{
		AudioSource = GetComponent<AudioSource>();
	}

	protected void PlayClip(AudioClip clip)
	{
		AudioSource.Stop();
		AudioSource.clip = clip;
		AudioSource.Play();
	}

	// Methods for calling SFX
	public void PlayCollectHealthPickup()
	{
		PlayClip(AudioclipHealthPickup);
	}

	public void HitByEnemy()
	{
		PlayClip(AudioclipDamageFromMonster);
	}

	public void HitByRespawnHazard()
	{
		PlayClip(AudioclipRespawnHazard);
	}

	public void Dead()
	{
		PlayClip(AudioclipDie);
	}
}
