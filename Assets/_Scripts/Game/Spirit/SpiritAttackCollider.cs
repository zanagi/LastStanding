using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritAttackCollider : MonoBehaviour {

    [HideInInspector] public Spirit spirit;
    
	void Start ()
    {
        spirit = GetComponentInParent<Spirit>();
	}
	
	private void OnTriggerEnter(Collider other)
    {
        if (spirit.state == SpiritState.Attack)
        {
            OnSpiritContact(other.GetComponentInParent<Spirit>());
            OnEnemyContact(other.GetComponentInParent<Enemy>());
        }
    }

    protected virtual void OnSpiritContact(Spirit other)
    {
        // 
    }

    protected virtual void OnEnemyContact(Enemy enemy)
    {
        if (enemy)
        {
            enemy.hitSound.Play();
            enemy.TakeDamage(spirit, spirit.strength);
            GameManager.Instance.uiCanvas.SpawnPEHitSound(
                GameManager.Instance.ScreenPos((spirit.transform.position + enemy.transform.position) / 2));
        }
    }
}
