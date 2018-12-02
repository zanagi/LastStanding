using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour {

    public bool resetHit;
    private bool spiritHit;
    private Enemy enemy;

    public void ResetCollider()
    {
        spiritHit = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!enemy)
            enemy = GetComponentInParent<Enemy>();

        if (!enemy.attacking)
            return;

        var spirit = other.GetComponent<Spirit>();
        if (spirit)
        {
            spirit.TakeDamage(enemy);
            spiritHit = resetHit;
        }
    }
}
