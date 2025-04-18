using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip idleSoundClip;
    [SerializeField] private AudioClip hitSoundClip;

    [SerializeField] private AudioClip deathSoundClip;
    [SerializeField] private AudioClip detectedSoundClip;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayLoopingIdle()
    {
        if (idleSoundClip != null && audioSource != null)
        {
            audioSource.clip = idleSoundClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayDetectedSound()
    {
        if (detectedSoundClip != null)
        {
            audioSource.PlayOneShot(detectedSoundClip);
        }
    }

    public void PlayHitSound()
    {
        if (hitSoundClip != null)
        {
            audioSource.PlayOneShot(hitSoundClip);
        }
    }

    public void PlayDeathSoundDetached()
    {
        if (deathSoundClip == null || audioSource == null) return;

        GameObject tempGO = new GameObject("TempAudio");
        AudioSource tempSource = tempGO.AddComponent<AudioSource>();

        tempSource.clip = deathSoundClip;
        tempSource.volume = audioSource.volume;
        tempSource.pitch = audioSource.pitch;
        tempSource.spatialBlend = audioSource.spatialBlend;
        tempSource.Play();

        Destroy(tempGO, deathSoundClip.length);
    }

}
