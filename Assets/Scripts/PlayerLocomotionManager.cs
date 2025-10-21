using System;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    PlayerManager player;

    public float verticalMovement, horizontalInput, moveAmount;

    private Vector3 moveDirection;
    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float runningSpeed = 5f;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        // Ground Movement
        HandleGroundedMovement();
        // Aerial Movement
    }

    private void GetVerticalAndHorizontalInput()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalInput = PlayerInputManager.instance.horizontalInput;


    }

    private void HandleGroundedMovement()
    {
        GetVerticalAndHorizontalInput();

        // Camera Relative Move Dir
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection += PlayerCamera.instance.transform.right * horizontalInput;
        moveDirection.y = 0f;
        moveDirection.Normalize();

        if (PlayerInputManager.instance.moveAmount > 0.5f)
        {
            // Move at running speed
            player.controller.Move(moveDirection * runningSpeed * Time.deltaTime);
        }
        else if (PlayerInputManager.instance.moveAmount <= 0.5f)
        {
            // Move at walking speed
            player.controller.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }
    }
}
 