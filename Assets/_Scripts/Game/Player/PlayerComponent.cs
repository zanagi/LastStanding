using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerComponent : MonoBehaviour
{
    public virtual void HandleUpdate(Player player) { }
    public virtual void HandleFixedUpdate(Player player) { }
}
