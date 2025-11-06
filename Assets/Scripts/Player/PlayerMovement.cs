using System;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    Player player;
    protected Vector2 moveInput;

    public Vector3 MoveDirection
    {
        get
        {
            GetMoveInput();
            GetMoveDirection();
            return moveDir;
        }
    }


    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }


    protected override void AerialMovement()
    {
        bool onGround = player.GroundCheck.OnGround();
        //Debug.Log(onGround);
        VerticalMovement(onGround);
    }

    

    

    void GetMoveInput()
    {
        moveInput = InputHandler.instance.MoveInput;
    }

    public void GetMoveDirection()
    {
        moveDir = PlayerCamera.instance.transform.forward * moveInput.y;
        moveDir += PlayerCamera.instance.transform.right * moveInput.x;
        moveDir.y = 0f;
        moveDir.Normalize();
    }

    protected override void GroundMovement()
    {
        GetMoveInput();
        GetMoveDirection();

        base.GroundMovement();
    }
}
