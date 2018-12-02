using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritAiCircle : SpiritAI {

    public float minRadius, maxRadius, angleSpeed = 180;
    private float angle, radius;
    private Vector3 startPos;
    private int direction;

    protected override void Start()
    {
        base.Start();
        startPos = transform.position;
        angle = Random.Range(0, 360);
        direction = Random.Range(0f, 1f) < 0.5f ? 1 : -1;
        radius = Random.Range(minRadius, maxRadius);
    }

    public override void HandleUpdate(Spirit spirit)
    {
        angle = (angle + angleSpeed * Time.deltaTime * direction);
        var targetPos = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        transform.LookAt(startPos + targetPos);
        var rBody = spirit.rBody;
        rBody.velocity = transform.forward * speed;
    }
}
