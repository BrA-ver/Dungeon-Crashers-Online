using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    protected Character character;
    protected CharacterController controller;

    

    

    [Header("Movement")]
    [SerializeField] protected float speed = 7f;
    [SerializeField] protected float rotationSpeed = 10;
    protected Vector3 moveDir;
    protected Vector3 velocity;
    protected Vector3 yVelocity;

    [Header("Grivity")]
    [SerializeField] protected float gravityScale;

    [Header("Jump")]
    [SerializeField] protected float jumpHeight = 5f;

    public Vector3 Velocity => velocity;

    protected Vector3 dashVector;
    protected float dashCounter;
    public bool dashing;

    protected bool onGround;

    protected virtual void Awake()
    {
        controller = GetComponent<CharacterController>();
        character = GetComponent<Character>();
    }

    public void SetMoveDirection(Vector3 direction)
    {
        moveDir = direction;
    }

    public void HandleAllMovement()
    {
        // Ground Movement
        GroundMovement();
        HandleRotation();
        // Aerial Movement
        AerialMovement();
    }

    protected virtual void AerialMovement()
    {
        VerticalMovement(onGround);
    }

    public void Jump(bool onGround)
    {
        if (!onGround) return;
        yVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravityScale * Physics.gravity.y);
    }

    public void VerticalMovement(bool onGround)
    {
        if (onGround && yVelocity.y <= 0f)
            yVelocity.y = 0f;
        else
            yVelocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;

        controller.Move(yVelocity * Time.deltaTime);
    }

    protected virtual void GroundMovement()
    {
        velocity = moveDir * speed;
        controller.Move(velocity * Time.deltaTime);
    }

    protected void HandleRotation()
    {
        if (moveDir.magnitude < 0.1f) return;

        Quaternion lookRotation = Quaternion.LookRotation(moveDir);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }

    public void HandleDash()
    {
        dashCounter -= Time.deltaTime;
        dashing = true;

        controller.Move(dashVector * Time.deltaTime);

        if (dashCounter <= 0f)
        {
            dashCounter = 0f;
            dashing = false;
        }
    }

    public void Dash(float dashSpeed, float dashTime)
    {
        dashCounter = dashTime;
        dashVector = transform.forward * dashSpeed;
        dashing = true;
    }

    public void Stop()
    {
        moveDir = Vector3.zero;
        velocity = Vector3.zero;
    }
}
