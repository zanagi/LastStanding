using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : SpiritAttackCollider {
    
    protected override void OnEnemyContact(Enemy enemy)
    {
        if (enemy)
        {
            GameManager.Instance.uiCanvas.SpawnPEHitSound(
                GameManager.Instance.ScreenPos((spirit.transform.position + enemy.transform.position) / 2));
            base.OnEnemyContact(enemy);
        }
    }
}
