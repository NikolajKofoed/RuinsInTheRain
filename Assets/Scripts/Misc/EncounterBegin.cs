using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EncounterBegin : MonoBehaviour
{
    [SerializeField] private GameObject BossEnemy; // Prefab to spawn
    [SerializeField] private Transform DroneSpawnPosition;
    [SerializeField] private AudioClip BossMusic;
    [SerializeField] private float fadeDuration = 1.5f;


    private BoxCollider2D invisibleWall;
    private GameObject spawnedBoss;
    private AudioSource bossAudioSource;
    private bool hasTrigged = false;

    private void Awake()
    {
        invisibleWall = GetComponent<BoxCollider2D>();
        bossAudioSource = gameObject.AddComponent<AudioSource>();
    }
    private void Start()
    {
        invisibleWall.enabled = false;
        bossAudioSource.playOnAwake = false;
        bossAudioSource.loop = true;
        bossAudioSource.volume = 0f; // Start muted for fade in
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTrigged)
        {
            hasTrigged = true;
            StartEncounter();
        }
    }

    public void StartEncounter()
    {
        if (BossEnemy == null) return;

        spawnedBoss = Instantiate(BossEnemy, DroneSpawnPosition.position, Quaternion.identity);
        invisibleWall.enabled = true;
        GetComponent<PolygonCollider2D>().enabled = false;

        if (BossMusic != null)
        {
            bossAudioSource.clip = BossMusic;
            bossAudioSource.Play();
            StartCoroutine(FadeInAudio());
        }

        CheckEncounterOver();
    }

    public void CheckEncounterOver()
    {
        if (spawnedBoss == null || !spawnedBoss.activeInHierarchy)
        {
            StartCoroutine(FadeOutAudio());
            GetComponent<Collider2D>().enabled = false;
            return;
        }

        StartCoroutine(WaitRoutine());
    }

    private IEnumerator WaitRoutine()
    {
        yield return new WaitForSeconds(1f); // check if encounter is over every 1 second
        CheckEncounterOver();
    }

    private IEnumerator FadeInAudio()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            bossAudioSource.volume = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        bossAudioSource.volume = 1f;
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = bossAudioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            bossAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        bossAudioSource.volume = 0f;
        bossAudioSource.Stop();
    }
}

