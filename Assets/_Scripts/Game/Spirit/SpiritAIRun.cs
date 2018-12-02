using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritAIRun : SpiritAI {

    public override void HandleUpdate(Spirit spirit)
    {
        var enemy = GetClosestEnemy();
        var delta = enemy.transform.position - transform.position;
        transform.LookAt(transform.position - delta);

        var rBody = spirit.rBody;
        rBody.velocity = transform.forward * speed;
    }
}
