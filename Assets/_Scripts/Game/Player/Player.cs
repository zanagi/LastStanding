﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Action
}

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerState playerState;

    private PlayerComponent[] components;
    private Spirit spirit;

    public float HealthRatio { get { return spirit.HealthRatio; } }
    public float SoulRatio { get { return spirit.SoulRatio; } }

    private void Start()
    {
        components = GetComponentsInChildren<PlayerComponent>(true);
        spirit = GetComponent<Spirit>();
    }

    public void HandleUpdate()
    {
        for (int i = 0; i < components.Length; i++)
            components[i].HandleUpdate(this);
    }

    public void HandleFixedUpdate()
    {
        for (int i = 0; i < components.Length; i++)
            components[i].HandleFixedUpdate(this);
    }
}
