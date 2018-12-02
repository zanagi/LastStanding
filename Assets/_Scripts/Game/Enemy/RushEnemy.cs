using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemy : Enemy
{
    [SerializeField] private AnimationCurve curve;

    protected override void AttackFrame(float t)
    {
        rBody.velocity = Vector3.zero;
        rBody.AddForce(transform.forward * curve.Evaluate(t / attackTime) * speed, ForceMode.VelocityChange);
    }
}
