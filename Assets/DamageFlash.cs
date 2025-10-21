using UnityEngine;
using System;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] float flashTime = 0.25f;
    [SerializeField] Material flashMaterial;
    Material[] originalMats;
    Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        originalMats = renderer.materials;
    }

    public void Flash()
    {
        StartCoroutine(DamgeFlashRoutine());
    }

    public void DoFlash()
    {
        Material[] flashList = new Material[1];
        flashList[0] = flashMaterial;
        renderer.materials = flashList;
        //renderer.materials[0] = flashMaterial;
    }

    public void StopFlash()
    {
        renderer.materials = originalMats;
    }

    IEnumerator DamgeFlashRoutine()
    {
        DoFlash();
        yield return new WaitForSeconds(flashTime);
        StopFlash();
    }
}
