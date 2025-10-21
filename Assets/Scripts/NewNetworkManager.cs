using UnityEngine;
using Unity.Netcode;
public class NewNetworkManager : NetworkBehaviour
{
    [Header("Position")]
    public NetworkVariable<Vector3> networkPosition = new NetworkVariable<Vector3>(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Vector3 networkPosVelocity;
    public float networkPosSmoothTIme = 0.1f;
}
