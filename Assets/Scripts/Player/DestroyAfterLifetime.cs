using UnityEngine;

public class DestroyAfterLifetime : MonoBehaviour
{
    [SerializeField] float lifeTime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
