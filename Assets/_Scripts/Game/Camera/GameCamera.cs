using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Third-person camera based on UnityStandardAssets PivotBasedCameraRig
public abstract class GameCamera : MonoBehaviour {

    public Camera Camera { get; protected set; }
    public Vector3 LookDirection { get { return Camera.transform.forward; } }
    public bool Animating { get; private set; }

    [SerializeField] protected Transform targetTransform;
    [SerializeField] protected float moveSpeed = 4;

    protected virtual void Start()
    {
        Camera = GetComponentInChildren<Camera>();
    }

    public Ray GetScreenRay(Vector3 pos)
    {
        return Camera.ScreenPointToRay(pos);
    }

    public abstract void HandleUpdate();
    public abstract void HandleFixedUpdate();
}
