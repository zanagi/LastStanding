using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : PlayerComponent
{
    public override void HandleUpdate()
    {
        if (player.State != SpiritState.Idle)
            return;

        if(Input.GetAxis("Attack") > 0)
        {
            player.State = SpiritState.Action;
            player.PlayAttackAnimation();
        }
    }

    public void EndAction()
    {
        player.State = SpiritState.Idle;
        player.PlayIdleAnimation();
    }
}
