using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public Rigidbody2D rb;
    public float _speed = 5f;
    public float _maxSpeed = 8f;
    public float _acceleration = 50f;
    public float _deceleration = 50f;
    public float _jumpStrenght = 9f;
    public bool _isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.position = Vector3.zero;
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float targetSpeed = horizontal * _speed;

        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float rate = horizontal != 0 ? _acceleration : _deceleration;

        //accelerate to maxSpeed
        rb.AddForce(new Vector2(speedDiff * rate, 0));

        //clamp speed to maxSpeed
        float clampedX = Mathf.Clamp(rb.linearVelocity.x, -_maxSpeed, _maxSpeed);
        rb.linearVelocity = new Vector2(clampedX, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            rb.AddForceY(_jumpStrenght, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plattform"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plattform"))
        {
            _isGrounded = false;
        }
    }
}
