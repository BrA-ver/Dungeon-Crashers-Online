using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LockOnCamera : MonoBehaviour
{
    PlayerCamera cam;

    public bool lockedOn = false;
    [SerializeField] Transform camPivot;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float pivotRotationSpeed = 10f;
    [SerializeField] float lockOnHieght = 7f;

    public LockOnTarget target;

    Vector3 startOffset;

    [SerializeField] float minPivot = -30f;
    [SerializeField] float maxPivot = 70f;

    [SerializeField] Image screenPosImage;

    [Header("Target Detection")]
    [SerializeField] float detectRadius = 4f;
    [SerializeField, Range(0f, 180f)] float FOV = 70f;
    [SerializeField] List<LockOnTarget> detectedTargets = new List<LockOnTarget>();
    [SerializeField] bool showRadiusGizmo;
    [SerializeField] bool showFovGizmo;

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
        Vector3 targetDir = target.transform.position - transform.position;
        targetDir.y = 0f;
        targetDir.Normalize();

        Quaternion lookRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        targetDir = target.transform.position - camPivot.position;
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
        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        screenPosImage.rectTransform.position = targetScreenPos;
        Debug.Log("Screen Pos: " + targetScreenPos);

        Debug.Log("Width: " + Screen.width);
        Debug.Log("Hieght: "+  Screen.height);

        if (targetScreenPos.y <= 0f)
        {
            StopLockOn();
        }
    }

    public void StopLockOn()
    {
        lockedOn = false;
        cam.SetRotationValues();
    }

    public void LockOn()
    {
        FindTargets();

        if (detectedTargets.Count > 0)
        {
            lockedOn = true;
            target = GetTargetNearestToScreenCenter();
        }
    }

    LockOnTarget GetTargetNearestToScreenCenter()
    {
        LockOnTarget closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (LockOnTarget target in detectedTargets)
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(target.transform.position);

            float differenceX = Mathf.Abs(targetScreenPos.x - screenCenter.x); 
            float differenceY = Mathf.Abs(targetScreenPos.y - screenCenter.y);

            float difference = new Vector2(differenceX, differenceY).magnitude;

            if (difference < closestDistance)
            {
                closestDistance = difference;
                closestTarget = target;
            }
        }

        return closestTarget;
    }

    void FindTargets()
    {
        // Use an overlap sphere to detect nearby colliders
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius);
        detectedTargets.Clear();
        Debug.Log(colliders.Length);

        // Loop through the colliders and, if the collider is a lock on target, add it to the targets list
        foreach (Collider collider in colliders)
        {
            LockOnTarget target = collider.GetComponent<LockOnTarget>();
            if (target == null)
                continue;

            Vector3 targetDirection = target.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, Camera.main.transform.forward);

            if (viewableAngle > -FOV && viewableAngle < FOV)
            {
                detectedTargets.Add(collider.GetComponent<LockOnTarget>());
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (showRadiusGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }

        if (showFovGizmo)
        {
            Quaternion myRotation = Quaternion.AngleAxis(-FOV, Vector3.up);
            Vector3 startDir = transform.forward;
            Vector3 minAngleVector = myRotation * startDir;

            Quaternion myRotatio2 = Quaternion.AngleAxis(FOV, Vector3.up);
            Vector3 maxAngleVector = myRotatio2 * startDir;

            Gizmos.DrawLine(transform.position, transform.position + (minAngleVector * detectRadius));
            Gizmos.DrawLine(transform.position, transform.position + (maxAngleVector * detectRadius));
        }
    }
}
