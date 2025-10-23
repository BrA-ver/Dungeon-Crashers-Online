using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    protected Character character;
    protected CharacterController controller;

    

    

    [Header("Movement")]
    [SerializeField] float speed = 7f;
    [SerializeField] float rotationSpeed = 10;
    Vector3 moveDir;
    Vector3 velocity;
    Vector3 yVelocity;

    [Header("Grivity")]
    [SerializeField] float gravityScale;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 5f;

    public Vector3 Velocity => velocity;

    Vector3 dashVector;
    float dashCounter;
    public bool dashing;

    bool onGround;

    protected virtual void Awake()
    {
        controller = GetComponent<CharacterController>();
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

    private void AerialMovement()
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

    void GroundMovement()
    {
        velocity = moveDir * speed;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleRotation()
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
}
