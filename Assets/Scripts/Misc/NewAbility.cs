using System.Collections;
using UnityEngine;
using TMPro;

public class NewAbility : MonoBehaviour
{
    // Ability number
    [SerializeField] private int Ability;

    // Sound
    [SerializeField] private AudioClip pickupSound;
    private AudioSource audioSource;

    // String reference for ability text GameObject (assigned by name in Inspector)
    [SerializeField] private string AbilityTextReference;

    // How long to show the ability text
    [SerializeField] private float showAbilityTextTimer = 5f;

    private Transform player;
    private ShowAbilityText showTextHandler;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Cache the ShowAbilityText handler
        showTextHandler = FindFirstObjectByType<ShowAbilityText>();

        if (string.IsNullOrEmpty(AbilityTextReference))
        {
            Debug.LogWarning("AbilityTextReference is not set on " + gameObject.name);
        }

        if (showTextHandler == null)
        {
            Debug.LogWarning("ShowAbilityText handler not found in the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PickUpSound();
            collision.GetComponent<Player2D>()?.UnlockAbility(Ability);

            if(showTextHandler == null)
            {
                showTextHandler = FindFirstObjectByType<ShowAbilityText>();
            }

            GameObject textObject = GameObject.Find(AbilityTextReference);
            if (textObject != null && showTextHandler != null)
            {
                var textComponent = textObject.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    showTextHandler.Show(textComponent, showAbilityTextTimer);
                }
                else
                {
                    Debug.LogWarning("TextMeshProUGUI component not found on " + AbilityTextReference);
                }
            }
            else
            {
                Debug.LogWarning("AbilityText GameObject or ShowAbilityText handler not found.");
            }

            Destroy(gameObject);
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

