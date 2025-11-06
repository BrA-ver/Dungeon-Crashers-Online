using UnityEngine;

public class BossMovement : CharacterMovement
{
    [Header("Sprinting")]
    [SerializeField] protected float sprintSpeed = 15f;

    public float Speed => speed;
    public float SprintSpeed => sprintSpeed;

    public void HandleAllMovement(float speed)
    {
        // Ground Movement
        GroundMovement(speed);
        HandleRotation(moveDir);
        // Aerial Movement
        AerialMovement();
    }

    public void GroundMovement(float moveSpeed)
    {
        velocity = moveDir * moveSpeed;
        controller.Move(velocity * Time.deltaTime);
    }
}
