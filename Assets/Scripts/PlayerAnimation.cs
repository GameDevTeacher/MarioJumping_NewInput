using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    private static readonly int Speed = Animator.StringToHash("speed");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

    // Start is called before the first frame update
    private void Start() => _animator = GetComponent<Animator>();

    public void SetAnimationParameters(Rigidbody2D rb, bool grounded)
    {
        // Animating the player
        _animator.SetFloat(Speed, Mathf.Abs(rb.velocity.x));
        _animator.SetBool(IsGrounded, grounded);
    }

    public void FlipThePlayer(Vector2 vector)
    {
        if (vector.x != 0f)
        {
            transform.localScale = new Vector3(vector.x, 1f, 1f); 
        }
    }
}
