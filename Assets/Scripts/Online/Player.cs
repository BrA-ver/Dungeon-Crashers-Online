using UnityEngine;

namespace Online
{
    public class Player : MonoBehaviour
    {
        PlayerMovement movement;
        public CharacterController controller;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            movement = GetComponent<PlayerMovement>();
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            movement.HandleAllMovement();
        }
    }
}