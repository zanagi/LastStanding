using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : PlayerComponent
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCastDist = 30.0f;
    [SerializeField] private bool lookAtCastTarget = false;

    public override void HandleUpdate()
    {
        if (player.State != SpiritState.Idle)
            return;

        if(Input.GetAxis("Attack") > 0)
        {
            player.State = SpiritState.Action;
            player.PlayAttackAnimation();

            if(lookAtCastTarget)
                SetDirection();
        }
    }

    public void EndAction()
    {
        player.State = SpiritState.Idle;
        player.PlayIdleAnimation();
    }

    public void SetPlayerAttack()
    {
        player.State = SpiritState.Attack;
    }

    // Set player to look at the direction mouse is pointing at
    private void SetDirection()
    {
        RaycastHit hit;
        GameCamera gameCamera = GameManager.Instance.gameCamera;
        
        if (Physics.Raycast(gameCamera.GetScreenRay(Input.mousePosition), out hit, groundCastDist, groundLayer))
        {
            var pos = hit.point;
            pos.y = 0;
            player.transform.LookAt(pos);
        }
    }
}
