using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Vector2 moveInput;
    Vector3 moveDir;
    Vector3 velocity;
    Vector3 yVelocity;

    Vector3 dashVector;

    [SerializeField] float speed = 7f;
    [SerializeField] float rotationSpeed = 10;

    [Header("Grivity")]
    [SerializeField] float gravityScale;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 5f;

    Player player;

    public Vector3 Velocity => velocity;

    float dashCounter;
    public bool dashing;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        player = GetComponent<Player>();
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
        bool onGround = player.GroundCheck.OnGround();
        Debug.Log(onGround);
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

        player.controller.Move(yVelocity * Time.deltaTime);
    }

    void GetMoveInput()
    {
        moveInput = InputHandler.instance.MoveInput;
    }

    private void GetMoveDirection()
    {
        moveDir = PlayerCamera.instance.transform.forward * moveInput.y;
        moveDir += PlayerCamera.instance.transform.right * moveInput.x;
        moveDir.y = 0f;
        moveDir.Normalize();
        
    }

    void GroundMovement()
    {
        GetMoveInput();
        GetMoveDirection();

        velocity = moveDir * speed;
        player.controller.Move(velocity * Time.deltaTime);

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

        player.controller.Move(dashVector * Time.deltaTime);

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
