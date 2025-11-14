using UnityEngine;
using UnityEngine.Events;

public class LockOnTarget : MonoBehaviour
{
    public UnityEvent destroyed;

    private void OnDestroy()
    {
        destroyed?.Invoke();
    }
}
