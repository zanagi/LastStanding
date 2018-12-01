using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Component that handles player movement/controls
public class PlayerMoveController : PlayerComponent
{
    public bool Moving { get; private set; }

    [SerializeField] private float speed = 4.0f;
    private Rigidbody rBody;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void HandleUpdate(Player player)
    {
        HandleMove(player);
    }

    public override void HandleFixedUpdate(Player player)
    {
    }

    private void HandleMove(Player player)
    {
        if (player.State != SpiritState.Idle)
            return;

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
            Moving = true;
        }
        else
        {
            rBody.velocity = new Vector3(0, rBody.velocity.y, 0);
            Moving = false;
        }
    }
}
