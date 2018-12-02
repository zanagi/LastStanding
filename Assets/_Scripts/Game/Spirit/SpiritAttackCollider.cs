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
            OnSpiritContact(other.GetComponent<Spirit>());
            OnEnemyContact(other.GetComponentInParent<Enemy>());
        }
    }

    protected virtual void OnSpiritContact(Spirit other)
    {
        if(other)
            other.ReceiveAttack(spirit);
    }

    protected virtual void OnEnemyContact(Enemy enemy)
    {
        if (enemy)
            enemy.TakeDamage(spirit, spirit.strength);
    }
}
