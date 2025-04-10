using System.Collections;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private enum PickupType
    {
        Currency
    }

    [SerializeField] private PickupType pickupType;
    [SerializeField] private float pickupDistance = 5f;
    [SerializeField] private float accelarationRate = .2f;
    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private float launchForce = 5f;
    [SerializeField] private float arcAngle = 45f; // In degrees

    private Vector3 moveDir;

    private Rigidbody2D rb;
    private bool hasLanded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        LaunchArc();
    }

    private void Update()
    {
        Vector3 playerPos = Player2D.Instance.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickupDistance)
        {
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelarationRate;
        }
        else
        {
            moveDir = Vector3.zero;
            moveSpeed = 0f;
        }
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = Player2D.Instance.transform.position;
        if (Vector3.Distance(transform.position, playerPos) < pickupDistance)
        {
            rb.linearVelocity = moveDir * moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player2D>())
        {
            DetectPickupType();
            Destroy(gameObject);
        }

        if (!hasLanded)
        {
            hasLanded = true;

            // Do something on landing — like play particles or stop movement
            Debug.Log("Landed!");

        }

    }

    public void LaunchArc()
    {
        hasLanded = false;

        // Choose a random angle within a small arc range
        float randomAngle = Random.Range(-arcAngle, arcAngle);
        float radians = randomAngle * Mathf.Deg2Rad;

        // Launch direction with an upward bias
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians) + 1f).normalized;

        // Apply force
        rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
    }

    private void DetectPickupType()
    {
        switch (pickupType)
        {
            case PickupType.Currency:
                EconomyManager.Instance.UpdateCurrentGold();
                break;
            default:
                break;
        }
    }
}
