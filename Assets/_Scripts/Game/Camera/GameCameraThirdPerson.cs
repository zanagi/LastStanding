using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Third-person camera based on UnityStandardAssets PivotBasedCameraRig
public class GameCameraThirdPerson : GameCamera {
    
    private Transform camTransform;
    private Transform pivotTransform;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float tiltMax = 75f;
    [SerializeField] private float tiltMin = 45f;
    [SerializeField] private bool lockCursor = false;

    private float lookAngle, tiltAngle;
    private Vector3 pivotAngle;
    private Quaternion pivotTargetRot, transformTargetRot;

    protected override void Start()
    {
        base.Start();

        // Init variables
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor;
        camTransform = Camera.transform;
        pivotTransform = camTransform.parent;
        pivotAngle = pivotTransform.localRotation.eulerAngles;
        pivotTargetRot = pivotTransform.localRotation;
        lookAngle = -pivotAngle.y;
        transformTargetRot = transform.localRotation;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void FollowTarget(float time)
    {
        if (targetTransform == null)
            return;
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, time * moveSpeed);
    }

    public override void HandleUpdate()
    {
        HandleRotationMovement();
    }

    public override void HandleFixedUpdate()
    {
        FollowTarget(Time.deltaTime);
    }

    private void HandleRotationMovement()
    {
        if (Time.timeScale < float.Epsilon)
            return;

        // Read the user input
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");
        lookAngle += x * turnSpeed;
        tiltAngle -= y * turnSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, -tiltMin, tiltMax);
        ApplyRotation();
    }

    public void ApplyRotation()
    {
        transformTargetRot = Quaternion.Euler(0f, lookAngle, 0f);
        pivotTargetRot = Quaternion.Euler(tiltAngle, pivotAngle.y, pivotAngle.z);
        pivotTransform.localRotation = pivotTargetRot;
        transform.localRotation = transformTargetRot;
    }
}
