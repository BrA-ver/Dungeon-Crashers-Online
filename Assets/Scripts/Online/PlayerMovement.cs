using UnityEngine;

namespace Online
{
    public class PlayerMovement : MonoBehaviour
    {
        Player player;

        Vector2 moveInput;
        Vector3 moveDirection;

        [SerializeField] float speed;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        public void HandleAllMovement()
        {
            moveInput = InputHandler.instance.MoveInput;
            // Handle Ground Movement
            HndleGroundMovement();
            // Handle Aeirial Movement
        }

        void HndleGroundMovement()
        {
            moveDirection = PlayerCamera.instance.transform.forward * moveInput.y;
            moveDirection += PlayerCamera.instance.transform.right * moveInput.x;
            moveDirection.y = 0f;
            moveDirection.Normalize();

            player.controller.Move(moveDirection * speed * Time.deltaTime);
        }
    }
}