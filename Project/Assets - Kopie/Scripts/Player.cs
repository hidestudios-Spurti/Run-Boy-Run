using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public Rigidbody2D rb;
    public float _currentSpeed = 0f;
    public float _maxSpeed = 10f;
    public float _boostSpeed = 15;
    public float _acceleration = 5f;
    public float _deceleration = 10f;
    public float _turnDeceleration = 40f;
    public float _jumpStrenght = 9f;
    public bool _isGrounded = false;
    public float _boostEndTime = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.position = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            rb.AddForceY(_jumpStrenght, ForceMode2D.Impulse);
        }   
        
    }



    private void FixedUpdate()
            {
                Movement();
            }

    void Movement()
    {
         float horizontal = Input.GetAxis("Horizontal");
        bool pressingOpposite = Mathf.Sign(horizontal) != Mathf.Sign(_currentSpeed) && horizontal != 0 && Mathf.Abs(_currentSpeed) > 0.1f;

        if (pressingOpposite)
        {
            //decelerate to 0 then accelerate the other way
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, _turnDeceleration * Time.fixedDeltaTime);
        }
        else if (horizontal != 0)
        {            
            //accelerate
            _currentSpeed += horizontal * _acceleration * Time.fixedDeltaTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, -_maxSpeed, _maxSpeed);
        }
        else
        {
            //decelerate
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, _deceleration *  Time.fixedDeltaTime);
        }

        if (Input.GetKey(KeyCode.LeftShift) && horizontal > 0)
        {
            _boostSpeed = 15f;
            _maxSpeed = 15f;
            _currentSpeed = _boostSpeed;
            _boostEndTime = Time.time + 5f;
            
        }
        
        if (Input.GetKey(KeyCode.LeftShift) && horizontal < 0 )
        {
            _boostSpeed = 15f;
            _maxSpeed = 15f;
            _currentSpeed = -_boostSpeed;
            _boostEndTime = Time.time + 5f;
            
        }

        if ( Time.time >= _boostEndTime)
        {
            _maxSpeed = 10f;
        }
        
        rb.linearVelocity = new Vector2(_currentSpeed, rb.linearVelocity.y);
       
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
