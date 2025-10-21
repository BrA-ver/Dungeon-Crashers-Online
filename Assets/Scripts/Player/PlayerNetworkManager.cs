using UnityEngine;
using Unity.Netcode;

public class PlayerNetworkManager : NetworkBehaviour
{
    Player player;

    [Header("Position")]
    public NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Vector3 VelocityRef;
    public float smoothTime = .25f;
    public float rotSmoothTime = .25f;

    [Header("Animator")]
    public NetworkVariable<bool> IsMoving = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> OnGround = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Awake()
    {
        player = GetComponent<Player>();
    }


    [ServerRpc]
    public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID)
    {
        if (IsServer) // If this object is the host, run the client rpc to update every client
        {
            PlayActionAnimationForAllClientsClientRpc(clientID, animationID);
        }
    }

    [ClientRpc] // A client rpc is sent to all clients from the server/host
    public void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID)
    {
        // When the host gets the client rpc, stop them from playing the animation again
        if (clientID != NetworkManager.Singleton.LocalClientId)
        {
            PerformActionAnimationFromServer(animationID);
        }
    }

    private void PerformActionAnimationFromServer(string animationID)
    {
        player.AnimationHandler.PlayTargetAnimation(animationID);
    }
}
