using System;
using System.Collections;
using UnityEngine;

// Handles camera clipping, based on UnityStandardAssets ProtectCameraFromWallClip
public class CameraClip : MonoBehaviour
{
    public bool Protecting { get; private set; }

    public float clipMoveTime, returnTime, sphereCastRadius, closestDistance;           
    public string dontClipTag = "Player";
    public Transform cameraTransform, pivotTransform; 

    private float originalDist, moveVelocity, currentDist;
    private Ray ray = new Ray();
    private RaycastHit[] hits;
    private RayHitComparer comparer;

    private void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        pivotTransform = cameraTransform.parent;
        originalDist = cameraTransform.localPosition.magnitude;
        currentDist = originalDist;
        comparer = new RayHitComparer();
    }

    private void LateUpdate()
    {
        float targetDist = originalDist;
        ray.origin = pivotTransform.position + pivotTransform.forward * sphereCastRadius;
        ray.direction = -pivotTransform.forward;
        
        var cols = Physics.OverlapSphere(ray.origin, sphereCastRadius);
        bool initialIntersect = false, hitSomething = false;
        
        for (int i = 0; i < cols.Length; i++)
        {
            if ((!cols[i].isTrigger) &&
                !(cols[i].attachedRigidbody != null && cols[i].attachedRigidbody.CompareTag(dontClipTag)))
            {
                initialIntersect = true;
                break;
            }
        }
        
        if (initialIntersect)
        {
            ray.origin += pivotTransform.forward * sphereCastRadius;
            hits = Physics.RaycastAll(ray, originalDist - sphereCastRadius);
        }
        else
        {
            hits = Physics.SphereCastAll(ray, sphereCastRadius, originalDist + sphereCastRadius);
        }
        Array.Sort(hits, comparer);
        float nearest = Mathf.Infinity;
        
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].distance < nearest && (!hits[i].collider.isTrigger) &&
                !(hits[i].collider.attachedRigidbody != null &&
                  hits[i].collider.attachedRigidbody.CompareTag(dontClipTag)))
            {
                nearest = hits[i].distance;
                targetDist = -pivotTransform.InverseTransformPoint(hits[i].point).z;
                hitSomething = true;
            }
        }
        
        Protecting = hitSomething;
        currentDist = Mathf.SmoothDamp(currentDist, targetDist, ref moveVelocity,
                                       currentDist > targetDist ? clipMoveTime : returnTime);
        currentDist = Mathf.Clamp(currentDist, closestDistance, originalDist);
        cameraTransform.localPosition = -Vector3.forward * currentDist;
    }
    
    public class RayHitComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
        }
    }
}
