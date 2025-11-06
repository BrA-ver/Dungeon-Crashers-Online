using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    public Player player { get; set; }
    public LockOnCamera lockOnCam;

    [Header("Camera Follow")]
    [SerializeField] float cameraSmoothSpeed = 1f;
    Vector3 cameraVelocity;

    [Header("Camera Rotation")]
    [SerializeField] public float horiLookAngle;
    [SerializeField] public float vertLookAngle;
    [SerializeField] float horiLookSpeed = 220f;
    [SerializeField] float vertLookSpeed = 220f;
    [SerializeField] Transform camPivot;
    [SerializeField] float minPivot = -30f;
    [SerializeField] float maxPivot = 70f;

    [Header("Camera Collision")]
    [SerializeField] Camera cam;
    [SerializeField] float camCollisionRad = 0.2f;
    [SerializeField] LayerMask collisionLayers;

    [Header("Death")]
    [SerializeField] float cameraDistanceWhileDead;

    Vector3 camPos;
    private float camStartZpos;
    private float targetZPos;

    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        lockOnCam = GetComponent<LockOnCamera>();
    }

    private void Start()
    {
        camStartZpos = cam.transform.localPosition.z;
    }

    private void LateUpdate()
    {
        HandleCameraActions();
    }

    void HandleCameraActions()
    {
        if (lockOnCam.lockedOn) return;
        if (!player) return;
        if (!player.isDead)
        {
            FollowTarget();
            HandleRotation();
            HandleCollisions();
        }
        else
        {
            // Lerp the camera's z position to the target position and pass it into the camera
            camPos.z = Mathf.Lerp(cam.transform.localPosition.z, cameraDistanceWhileDead, 0.2f);
            cam.transform.localPosition = camPos;

            Vector3 cameraRotation = Vector3.zero;
            cameraRotation.x = maxPivot;
            Quaternion targetRotatiation = Quaternion.Euler(cameraRotation);
            camPivot.localRotation = Quaternion.Slerp(camPivot.localRotation, targetRotatiation, 5f * Time.deltaTime);
        }
    }

    public void FollowTarget()
    {
        Vector3 targetCamPos = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCamPos;
    }

    void HandleRotation()
    {
        horiLookAngle += (InputHandler.instance.LookInput.x * horiLookSpeed) * Time.deltaTime;
        vertLookAngle -= (InputHandler.instance.LookInput.y * vertLookSpeed) * Time.deltaTime;

        vertLookAngle = Mathf.Clamp(vertLookAngle, minPivot, maxPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotatiation;
        // Rotates THIS game object on the y-axis
        cameraRotation.y = horiLookAngle;
        targetRotatiation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotatiation;

        // Rotate the camera pivot on the x-axis, up and down
        cameraRotation = Vector3.zero;
        cameraRotation.x = vertLookAngle;
        targetRotatiation = Quaternion.Euler(cameraRotation);
        camPivot.localRotation = targetRotatiation;
    }

    public void HandleCollisions()
    {
        // Initialize where the camera wants to be and a raycast hit
        targetZPos = camStartZpos;
        RaycastHit hit;

        // Get the diection to the camera object
        Vector3 direction = cam.transform.position - camPivot.transform.position;
        direction.Normalize();

        // Do a sphere cast at the pivot in the direction of the camera and output to the hit and use the the target camera offset as the max distance
        if (Physics.SphereCast(camPivot.position, camCollisionRad, direction, out hit, Mathf.Abs(targetZPos), collisionLayers))
        {
            //Debug.Log("Detected");
            // Get the distance to the hit point from the pivot 
            float distanceToHitObject = Vector3.Distance(camPivot.position, hit.point);
            // The difference between the the distance and the collsion radius is our target offset. 
            // Invert the target offset so the camera moves forward instead of backwards
            targetZPos = -(distanceToHitObject - camCollisionRad);
        }

        // If the distance from the hit object is smaller than the collision radius. move forward using the collision radius
        if (Mathf.Abs(targetZPos) < camCollisionRad)
        {
            targetZPos = -camCollisionRad;
        }

        // Lerp the camera's z position to the target position and pass it into the camera
        camPos.z = Mathf.Lerp(cam.transform.localPosition.z, targetZPos, 0.2f);
        cam.transform.localPosition = camPos;
    }

    internal void SetPlayer(Player player)
    {
        this.player = player;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(camPivot.position, camCollisionRad);
    }

    public void SetRotationValues()
    {
        horiLookAngle = transform.eulerAngles.y;
        vertLookAngle = camPivot.localEulerAngles.x;

        if (vertLookAngle > 180)
        {
            vertLookAngle -= 360f;
        }

    }
}