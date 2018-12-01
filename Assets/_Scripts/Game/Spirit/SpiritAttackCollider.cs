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
            var otherSpirit = other.GetComponent<Spirit>();

            if (otherSpirit)
            {
                OnSpiritContact(otherSpirit);
            }
        }
    }

    protected virtual void OnSpiritContact(Spirit other)
    {
        other.ReceiveAttack(spirit);
    }
}
