using System;
using UnityEngine;
using UnityEngine.UI;

public class LockOnCamera : MonoBehaviour
{
    PlayerCamera cam;

    public bool lockedOn = false;
    [SerializeField] Transform camPivot;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float pivotRotationSpeed = 10f;
    [SerializeField] float lockOnHieght = 7f;

    public Transform target;

    Vector3 startOffset;

    [SerializeField] float minPivot = -30f;
    [SerializeField] float maxPivot = 70f;

    [SerializeField] Image screenPosImage;

    private void Start()
    {
        startOffset = camPivot.localPosition;
        cam = PlayerCamera.instance;
    }

    private void LateUpdate()
    {
        if (!lockedOn)
            return;

        LookAtTarget();
        cam.FollowTarget();
        cam.HandleCollisions();
        CheckForScreenExit();
    }

    private void LookAtTarget()
    {
        // Get the direction to the target position
        Vector3 targetDir = target.position - transform.position;
        targetDir.y = 0f;
        targetDir.Normalize();

        Quaternion lookRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        targetDir = target.position - camPivot.position;
        lookRotation = Quaternion.LookRotation(targetDir);
        float xRot = lookRotation.eulerAngles.x;

        if (xRot > 180f) xRot -= 360f;

        Debug.Log(xRot);
        xRot = Mathf.Clamp(xRot, minPivot, maxPivot);
        Debug.Log(xRot + " After");
        lookRotation = Quaternion.Euler(xRot, lookRotation.eulerAngles.y, lookRotation.eulerAngles.z);
        camPivot.rotation = Quaternion.Slerp(camPivot.rotation, lookRotation, pivotRotationSpeed * Time.deltaTime);

    }

    void CheckForScreenExit()
    {
        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(target.position);
        screenPosImage.rectTransform.position = targetScreenPos;
        Debug.Log("Screen Pos: " + targetScreenPos);

        Debug.Log("Width: " + Screen.width);
        Debug.Log("Hieght: "+  Screen.height);

        if (targetScreenPos.y <= 0f)
        {
            StopLockOn();
        }
    }

    void StopLockOn()
    {
        lockedOn = false;
        cam.SetRotationValues();
    }
}
