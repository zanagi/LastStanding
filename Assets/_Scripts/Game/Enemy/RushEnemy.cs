using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemy : Enemy
{
    [SerializeField] private AnimationCurve curve;

    protected override IEnumerator Attack()
    {
        // Rotation attack
        var t = 0.0f;
        while (t < attackTime)
        {
            t += Time.fixedDeltaTime;
            rBody.velocity = Vector3.zero;
            rBody.AddForce(transform.forward * curve.Evaluate(t / attackTime) * speed, ForceMode.VelocityChange);
            yield return new WaitForFixedUpdate();
        }
    }
}
