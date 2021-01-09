using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f, jumpForce = 20f;
    
    [Space(5f)]
    public LayerMask whatIsGround;
    
    private Rigidbody2D _rigidbody2D;
    private PlayerAnimation _playerAnimation;
 
    private Vector2 _movementVector;

    private bool _isJumping = false;

    private float _jumpTimeCounter;
    
    [SerializeField] private float jumpTime = 0.25f;
    
    private bool _jumpPressed;

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }
    
    // Get Movement Input
    private void OnMove(InputValue movementValue)
    {
        var movementVector = movementValue.Get<Vector2>();
        _movementVector.x = movementVector.x;
    }

    // Get Jump Input
    private void OnJump()
    {
        // If we are not grounded, do not go further in the code.
        if (!IsGrounded()) 
            return;
        _isJumping = true;
        _jumpTimeCounter = jumpTime;
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
    }

    // Using a separate event to get button value (0 or 1) This tells us if the button is currently held in or not
    private void OnJumpHeld(InputValue jumpValue)
    {
        // Get the float value of the button which will be 0 if not pressed and 1 if pressed.
        var value = jumpValue.Get<float>();
        // this is a short if statement. We set _jumpPressed to be the answer(true or false) of the statement
            // The statement being that value is greater or equal to 1.
            
        _jumpPressed = value >= 1f; // This is a slightly more condensed version. //_jumpPressed = jumpValue.Get<float>() >= 1f;
    }

    // Update is called once per frame
    private void Update()
    {
        #region Animation

        // Animate the player using parameters from the player input and controller.
        _playerAnimation.SetAnimationParameters(_rigidbody2D, IsGrounded());
        _playerAnimation.FlipThePlayer(_movementVector);

        #endregion

        #region Jumping
        if (_jumpPressed && _isJumping)
        {
            if (_jumpTimeCounter > 0)
            {
                _rigidbody2D.velocity = Vector2.up * jumpForce;
                _jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                _isJumping = false;
            }
        }

        if (!_jumpPressed)
        {
            _isJumping = false;
        }
        #endregion
    }

    // FixedUpdate is called 50 times per second
    private void FixedUpdate()
    {
        //Move the player
        _rigidbody2D.velocity = new Vector2(_movementVector.x * moveSpeed, _rigidbody2D.velocity.y);
    }

    // Check if player is on ground
    private bool IsGrounded()
    {
        print("Is being run");
        var position = transform.position;
        var direction = Vector2.down;
        const float distance = 1.5f;
        
        Debug.DrawRay(position, direction, new Color(1f, 0f, 1f));
        var hit = Physics2D.Raycast(position, direction, distance, whatIsGround);
        
        return hit.collider != null;
    }
}