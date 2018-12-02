using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    public string idleStateName = "Idle", moveStateName = "Run", attackStateName = "Attack";
    public float moveTransitionTime = 0.2f, stopTransitionTime = 0.5f, attackTransitionTime = 0.2f;
    private string targetState;
    private float targetTime;

    private PlayerComponent[] components;
    private Spirit spirit;

    public float HealthRatio { get { return spirit.HealthRatio; } }
    public float SoulRatio { get { return spirit.SoulRatio; } }
    public SpiritState State { get { return spirit.state; } set { spirit.state = value; }  }

    private void Start()
    {
        components = GetComponentsInChildren<PlayerComponent>(true);
        spirit = GetComponent<Spirit>();
        animator = GetComponent<Animator>();
    }

    public void HandleUpdate()
    {
        for (int i = 0; i < components.Length; i++)
            components[i].HandleUpdate();
    }

    public void HandleFixedUpdate()
    {
        for (int i = 0; i < components.Length; i++)
            components[i].HandleFixedUpdate();
    }

    public void HandleLateUpdate()
    {
        if (targetState.Length > 0)
        {
            animator.CrossFade(targetState, targetTime);
            targetState = string.Empty;
        }
    }

    public void PlayMoveAnimation()
    {
        if (moveStateName != attackStateName)
        {
            targetState = moveStateName;
            targetTime = moveTransitionTime;
        }
    }

    public void PlayIdleAnimation()
    {
        if (moveStateName != attackStateName)
        {
            targetState = idleStateName;
            targetTime = stopTransitionTime;
        }
    }

    public void PlayAttackAnimation()
    {
        targetState = attackStateName;
        targetTime = attackTransitionTime;
    }
}
