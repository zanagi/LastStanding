using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerComponent : MonoBehaviour
{
    protected Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public virtual void HandleUpdate() { }
    public virtual void HandleFixedUpdate() { }
}
