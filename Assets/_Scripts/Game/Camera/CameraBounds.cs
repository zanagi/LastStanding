using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour {

    [SerializeField]
    private Vector3 min;
    [SerializeField]
    private Vector3 max;

    public Vector3 GetPosInBounds(Vector3 newPos, Vector3 offset)
    {
        var pos = newPos;
        pos.x = Mathf.Clamp(pos.x, min.x + offset.x, max.x + offset.x);
        pos.z = Mathf.Clamp(pos.z, min.z + offset.z, max.z + offset.z);
        return pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red * 0.5f;
        Gizmos.DrawCube(0.5f * (min + max) + transform.position + Vector3.up * 0.01f, max - min + Vector3.up * 0.01f);
    }
}
