using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour {

    private static int maxHealth = 100, maxSoul = 100, levelExp = 100;

    [Range(0, 100)] public int health = 100;
    [Range(0, 100)] public int soul = 100;

    [Header("Level")]
    public int level = 1;
    private int exp = 0;

    [Header("Class specific values")]
    public float soulModifier = 1.0f;

    [HideInInspector]
    public SpiritState state;
    
    // Glow/Shader variables
    private static string soulEffectName = "_RimPower";
    private static float maxSoulEffect = 1, minSoulEffect = 4;
    private Material[] materials;
    private int previousSoul;

    public float HealthRatio
    {
        get { return (float)health / maxHealth; }
    }

    public float SoulRatio
    {
        get { return (float)soul / maxSoul; }
    }

    private void Start()
    {
        var renderers = GetComponentsInChildren<Renderer>();

        // Set materials
        materials = new Material[renderers.Length];
        for(int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
        }
        SetGlow();
        previousSoul = soul;
    }

    private void SetGlow()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat(soulEffectName, Mathf.Lerp(minSoulEffect, maxSoulEffect, soul / 100.0f));
        }
    }

    protected virtual void Update()
    {
        if (!GameManager.Instance.IsIdle)
            return;

        CheckSoul();
        CheckHealth();
    }

    private void CheckHealth()
    {
        if(health <= 0)
        {
            Explode();
        }
    }

    private void CheckSoul()
    {
        if (previousSoul != soul)
            SetGlow();
        previousSoul = soul;
    }


    public void AddSoul(int amount)
    {
        if (amount <= 0)
            return;
        soul = Mathf.Clamp(soul + amount, 0, 100);
    }

    public void ReduceSoul(int amount)
    {
        if (amount <= 0)
            return;
        soul = Mathf.Clamp(soul - (int)(soulModifier * amount), 0, 100);
    }
    
    public void AddExp(int amount)
    {
        if (amount <= 0)
            return;
        exp += amount;

        if (exp >= levelExp)
        {
            exp -= levelExp;
            level++;
        }
    }

    public void Explode()
    {
        // TODO:
    }
}
