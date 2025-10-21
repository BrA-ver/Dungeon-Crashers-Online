using UnityEngine;
using Unity.Netcode;

public class PlayerManager : NetworkBehaviour
{
    PlayerLocomotionManager playerLocomotion;
    public CharacterController controller;

    NewNetworkManager networkManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        playerLocomotion = GetComponent<PlayerLocomotionManager>();
        controller = GetComponent<CharacterController>();
        networkManager = GetComponent<NewNetworkManager>();
    }

    private void Update()
    {
        if (IsOwner)
        {
            networkManager.networkPosition.Value = transform.position;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, networkManager.networkPosition.Value, ref networkManager.networkPosVelocity, networkManager.networkPosSmoothTIme);
        }

        if (!IsOwner)
            return;

        playerLocomotion.HandleAllMovement();
    }
}
