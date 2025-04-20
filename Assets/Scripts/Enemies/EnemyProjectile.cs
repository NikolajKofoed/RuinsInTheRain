using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private float projectileRange = 10f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }
    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Health player = collision.gameObject.GetComponent<Health>();

        if (!collision.isTrigger && player)
        {
            if ((player))
            {
                player?.TakeDamage(1, transform);
                Destroy(gameObject);
            }
            else if (!collision.isTrigger)
            {
                Destroy(gameObject);
            }
        }
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
}
