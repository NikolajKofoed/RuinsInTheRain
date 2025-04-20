using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public int projectileDamage = 2;
    private float direction;
    private bool hit;
    private float lifetime;

    private Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale; // Cache original scale
    }

    void Update()
    {
        if (hit) return;

        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) { gameObject.SetActive(false); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;

        if(collision.CompareTag("Enemy"))
        {
            Deactivate();
            collision.GetComponent<EnemyHealth>()?.TakeDamage(projectileDamage, this.transform);
        }

        Deactivate();

        if (gameObject.activeSelf)
        {
            StartCoroutine(DeactiveProjectileTimerRoutine()); // failsafe
        }
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        hit = false;
        gameObject.SetActive(true);

        // Always flip based on initial scale
        transform.localScale = new Vector3(initialScale.x * _direction, initialScale.y, initialScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator DeactiveProjectileTimerRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
