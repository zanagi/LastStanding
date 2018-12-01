using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraTopDown : GameCamera {
    
    protected Vector3 cameraOffset;

    protected override void Start()
    {
        base.Start();
        cameraOffset = transform.position - targetTransform.position;
    }

    public override void HandleUpdate()
    {
    }

    public override void HandleFixedUpdate()
    {
        Move(Time.fixedDeltaTime);
    }

    protected virtual void Move(float time)
    {
        var targetPos = targetTransform.position + cameraOffset;
        var smoothPos = Vector3.Lerp(transform.position, targetPos, moveSpeed * time);
        transform.position = smoothPos;
    }

    public Ray GetScreenRay(Vector3 pos)
    {
        return Camera.ScreenPointToRay(pos);
    }
}
