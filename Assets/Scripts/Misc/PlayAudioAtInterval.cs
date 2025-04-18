using System.Collections;
using UnityEngine;

public class PlayAudioAtInterval : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float minIntervalValue;
    [SerializeField] private float maxIntervalValue;

    [SerializeField] private AudioClip[] audioClips;

    private bool isPlayingAudioClips = false;

    private void Start()
    {
        PlayAudioClips();
    }

    public void PlayAudioClips()
    {
        if (audioClips.Length == 0 || isPlayingAudioClips) return;

        StartCoroutine(PlayAudioClipsRoutine());
    }

    private IEnumerator PlayAudioClipsRoutine()
    {
        isPlayingAudioClips = true;

        while (true)
        {
            int index = Random.Range(0, audioClips.Length);
            AudioClip clip = audioClips[index];

            yield return new WaitForSeconds(Random.Range(minIntervalValue, maxIntervalValue));

            audioSource.PlayOneShot(clip);

        }
    }

}
