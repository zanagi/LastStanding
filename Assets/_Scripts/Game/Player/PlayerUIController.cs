using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : PlayerComponent {

    [SerializeField] private UIBar healthBar;
    [SerializeField] private UIBar soulBar;

    public override void HandleUpdate()
    {
        healthBar.SetFill(player.HealthRatio);
        soulBar.SetFill(player.SoulRatio);
    }
}
