using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public int projectileDamage = 2;
    private float direction;
    private bool hit;
    private float lifetime;

    //private Animator anim;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        //anim = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
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
        circleCollider.enabled = false;
        //anim.SetTrigger("explode");
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<EnemyHealth>().TakeDamage(projectileDamage); // change later
        }
        Deactivate(); //temp because I have no animation
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        circleCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if(Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
