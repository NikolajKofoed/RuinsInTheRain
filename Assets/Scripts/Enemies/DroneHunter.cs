using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

public class DroneHunter : MonoBehaviour, IEnemy
{
    // Movement
    [SerializeField] private float speed;
    [SerializeField] private float idleLineOfSight;
    [SerializeField] private float huntingLineOfSight;
    [SerializeField] private Transform SpotlightPosRight;
    [SerializeField] private Transform SpotlightPosLeft;
    [SerializeField] private AudioClip DetectionSound;
    private float lineOfSight;
    private bool hunterMode = false;
    private bool canPlayDetectionSound = false;

    private Rigidbody2D rb;
    private Transform player;
    private SpriteRenderer spriteRenderer; // new
    private Light2D spotlight;
    private AudioSource audioSource;

    [field: SerializeField] private float damage;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spotlight = GetComponentInChildren<Light2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // cache it
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {

    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer < lineOfSight)
        {
            lineOfSight = huntingLineOfSight;
            hunterMode = true;
            if (canPlayDetectionSound)
            {
                canPlayDetectionSound = false;
                PlayDetectionSound();
                Debug.Log("played detection sound");
            }
        }
        else
        {
            lineOfSight = idleLineOfSight;
            hunterMode = false;
            canPlayDetectionSound = true;
        }


    }

    private void FixedUpdate()
    {
        if (hunterMode)
        {
            transform.position = Vector2.MoveTowards(rb.position, player.position, speed * Time.deltaTime);
            bool facingLeft = player.position.x < transform.position.x;

            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = facingLeft;
            }

            if (spotlight != null)
            {
                float spotlightAngle;

                if (facingLeft)
                {
                    spotlightAngle = 90f;
                    spotlight.transform.position = SpotlightPosLeft.transform.position;
                }
                else
                {
                    spotlightAngle = -90f;
                    spotlight.transform.position = SpotlightPosRight.transform.position;

                }

                spotlight.transform.rotation = Quaternion.Euler(0f, 0f, spotlightAngle);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().TakeDamage(damage, transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, idleLineOfSight);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, huntingLineOfSight);
    }

    public void Attack()
    {
        // Optional: Add attack logic here
    }

    private void PlayDetectionSound()
    {
        if(DetectionSound != null)
        {
            audioSource.PlayOneShot(DetectionSound);
            Debug.Log("detection sound was not null");
        }
    }
}

