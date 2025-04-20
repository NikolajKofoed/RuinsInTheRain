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
    private float lineOfSight;
    private bool hunterMode = false;
    private bool previousHunterMode = false; // track last state

    private Rigidbody2D rb;
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Light2D spotlight;

    private EnemyAudio enemyAudio; // reference to EnemyAudio

    [field: SerializeField] private float damage;

    private void Awake()
    {
        spotlight = GetComponentInChildren<Light2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (enemyAudio == null)
        {
            enemyAudio = GetComponent<EnemyAudio>();
        }
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        previousHunterMode = hunterMode;
        hunterMode = distanceFromPlayer < lineOfSight;
        lineOfSight = hunterMode ? huntingLineOfSight : idleLineOfSight;

        // play detection sound only when entering hunter mode
        if (hunterMode && !previousHunterMode && enemyAudio != null)
        {
            enemyAudio.PlayDetectedSound();
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
                float spotlightAngle = facingLeft ? 90f : -90f;
                spotlight.transform.position = facingLeft ? SpotlightPosLeft.position : SpotlightPosRight.position;
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
}

