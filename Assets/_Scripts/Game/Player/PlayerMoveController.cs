using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Component that handles player movement/controls
public class PlayerMoveController : PlayerComponent
{
    public bool Moving { get; private set; }

    [SerializeField] private float speed = 4.0f;
    private Rigidbody rBody;
    
    [Header("Audio")]
    [SerializeField] private AudioSource footstepSource;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void HandleUpdate()
    {
        HandleMove(player);
    }

    public override void HandleFixedUpdate()
    {
    }

    private void HandleMove(Player player)
    {
        if (player.State != SpiritState.Idle)
        {
            Stop(player);
            return;
        }

        var h = Input.GetAxis(Static.horizontalAxis);
        var v = Input.GetAxis(Static.verticalAxis);
        
        if (h != 0 || v != 0)
        {
            var lookDir = GameManager.Instance.gameCamera.LookDirection;
            lookDir.y = 0;
            var angle = Vector2.SignedAngle(Vector2.up, new Vector2(h, v));
            var dir = Quaternion.Euler(0, -angle, 0) * lookDir;
            transform.LookAt(transform.position + dir);
            rBody.velocity = transform.forward * speed;

            if (!Moving)
            {
                Moving = true;
                player.PlayMoveAnimation();
            }
        }
        else
        {
            Stop(player);
            player.PlayIdleAnimation();
        }
    }

    private void Stop(Player player)
    {
        if (Moving)
        {
            Moving = false;
            rBody.velocity = new Vector3(0, rBody.velocity.y, 0);
        }
    }

    public void PlayFootstep()
    {
        if(footstepSource)
            footstepSource.Play();
    }
}
