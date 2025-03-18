using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    private PlayerControls playerControls;
    private Rigidbody2D rb;
    private Vector2 movement;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        playerControls.Movement.Jump.performed += ctx => Jump();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        //Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector2 newPosition = rb.position + new Vector2(movement.x * moveSpeed * Time.fixedDeltaTime, 0);
        rb.MovePosition(newPosition);
    }

    private void Jump()
    {

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Use velocity for jump
            Debug.Log("Jump was performed");

    }

}
