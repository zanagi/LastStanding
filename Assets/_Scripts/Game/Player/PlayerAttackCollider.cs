using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : SpiritAttackCollider {

    protected override void OnSpiritContact(Spirit other)
    {
        if (other)
            other.ReceiveAttack(spirit);
    }
}
