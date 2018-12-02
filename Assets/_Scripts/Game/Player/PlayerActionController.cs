using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionController : PlayerComponent
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCastDist = 30.0f;
    [SerializeField] private bool lookAtCastTarget = false;

    [SerializeField] private LayerMask spiritLayer;
    [SerializeField] private Transform frontBoxTransform;
    [SerializeField] private Vector3 frontBoxExtents;

    [SerializeField] private Transform dirArrowTransform;

    private void Start()
    {
        dirArrowTransform.position = player.transform.position;
    }

    public override void HandleUpdate()
    {
        if (player.State != SpiritState.Idle)
            return;

        CheckDirection();
        if (Input.GetAxis("Attack") > 0)
        {
            player.State = SpiritState.Action;
            player.PlayAttackAnimation();

            if(lookAtCastTarget)
                SetDirection();
            CheckFront();
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

    // Check if possible target in front
    private void CheckFront()
    {
        var colliders = Physics.OverlapBox(frontBoxTransform.position, frontBoxExtents, transform.rotation,
            spiritLayer);
        var pos = transform.position;
        
        Spirit targetSpirit = null;

        for (int i = 0; i < colliders.Length; i++)
        {
            var spirit = colliders[i].GetComponent<Spirit>();

            if (!targetSpirit || Vector3.SqrMagnitude(spirit.transform.position - pos)
                    < Vector3.SqrMagnitude(targetSpirit.transform.position - pos))
            {
                targetSpirit = spirit;
            }
        }
        if (targetSpirit && Random.Range(0f, 1f) < 0.5f)
        {
            GameManager.Instance.InitAttackScene(targetSpirit);
            return;
        }
    }

    private void CheckDirection()
    {
        // Set arrow pos
        dirArrowTransform.position = player.transform.position;
        RaycastHit hit;
        GameCamera gameCamera = GameManager.Instance.gameCamera;

        if (Physics.Raycast(gameCamera.GetScreenRay(Input.mousePosition), out hit, groundCastDist, groundLayer))
        {
            var pos = hit.point;
            pos.y = 0;
            dirArrowTransform.LookAt(pos);
        }
    }

    // Set player to look at the direction mouse is pointing at
    private void SetDirection()
    {
        var targetPos = transform.position + dirArrowTransform.forward;
        player.transform.LookAt(targetPos);
    }
}
