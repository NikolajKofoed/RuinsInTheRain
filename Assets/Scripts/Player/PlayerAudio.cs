using System.Collections;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] footstepClips;

    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip airJump;

    [SerializeField] private AudioClip meleeAttackClip;
    [SerializeField] private AudioClip rangedAttackClip;

    private Player2D player2D;


    private bool isPlayingFootstep = false;

    private void Awake()
    {
        player2D = GetComponent<Player2D>();
    }

    public void PlayFootstep()
    {
        if (footstepClips.Length == 0 || isPlayingFootstep) return;

        StartCoroutine(PlayFootstepRoutine());
    }

    private IEnumerator PlayFootstepRoutine()
    {
        isPlayingFootstep = true;
        var speed = player2D.Speed;

        // Randomize footstep clip
        int index = Random.Range(0, footstepClips.Length);
        AudioClip clip = footstepClips[index];

        // Adjust pitch based on speed
        audioSource.pitch = Mathf.Clamp(speed / 10f, 0.8f, 1.2f);

        // Play the footstep sound
        audioSource.PlayOneShot(clip);

        // Adjust delay between footsteps based on speed
        float stepDelay = Mathf.Clamp(1f / speed, 0.2f, 0.5f);

        yield return new WaitForSeconds(stepDelay);

        isPlayingFootstep = false;
    }


    public void PlayJump()
    {
        if (jumpClip != null)
        {
            audioSource.PlayOneShot(jumpClip);
        }
    }

    public void PlayAirJump()
    {
        if(airJump != null)
        {
            audioSource.PlayOneShot(airJump);
        }
    }


    public void PlayMeleeAttack()
    {
        if(meleeAttackClip != null)
        {
            audioSource.PlayOneShot(meleeAttackClip);
        }
    }

    public void PlayRangedAttack()
    {
        if(rangedAttackClip != null)
        {
            audioSource.PlayOneShot(rangedAttackClip);
        }
    }
}

