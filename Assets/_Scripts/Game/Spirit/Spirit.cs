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
    public float attackForce = 4.0f;

    [HideInInspector] public SpiritState state;
    [HideInInspector] public Rigidbody rBody;

    // Received attack from other
    [HideInInspector] public Spirit attacker;
    [HideInInspector] public float flightForce;
    [HideInInspector] public Vector3 flightDirection;
    private bool flightCheck = false;
    private float flightFriction = 5.0f, flightMass = 10.0f, normalMass,
        explosionLimit = 3;

    // Glow/Shader variables
    private static string soulEffectName = "_RimPower", baseColorName = "_BaseColor", glowColorName = "_RimColor";
    private static float maxSoulEffect = 1, minSoulEffect = 4, flightMinSqr = 0.01f;
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
        if (materials != null)
            return;

        rBody = GetComponent<Rigidbody>();
        normalMass = rBody.mass;

        // Set materials
        var renderers = GetComponentsInChildren<Renderer>();
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
        if (materials == null)
            Start();

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat(soulEffectName, Mathf.Lerp(minSoulEffect, maxSoulEffect, soul / 100.0f));
        }
    }

    public void SetColors(Color baseColor, Color glowColor)
    {
        if (materials == null)
            Start();

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor(baseColorName, baseColor);
            materials[i].SetColor(glowColorName, glowColor);
        }
    }

    protected virtual void Update()
    {
        if (!GameManager.Instance.IsIdle)
            return;

        CheckSoul();
        CheckHealth();
        CheckFlight();
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

    private void CheckFlight()
    {
        if (state == SpiritState.Flight)
        {
            if (flightForce <= 0 || (flightCheck && rBody.velocity.sqrMagnitude < flightMinSqr))
            {
                StopFlight();
                return;
            }
            rBody.velocity = flightDirection * flightForce;
            flightForce -= Time.deltaTime * flightFriction;
            flightCheck = true;
        }
    }

    private void StopFlight()
    {
        state = SpiritState.Idle;
        rBody.velocity = new Vector3(0, rBody.velocity.y, 0);
        rBody.mass = normalMass;
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

    public void ReceiveAttack(Spirit source)
    {
        attacker = source;
        var impulse = source.transform.forward * source.attackForce;
        flightForce = impulse.magnitude;
        flightDirection = impulse / flightForce;
        state = SpiritState.Flight;
        flightCheck = false;
        rBody.mass = flightMass;
    }

    public void Explode()
    {
        // TODO:
        Destroy(gameObject);
    }

    // Check flight explosion
    private void OnCollisionEnter(Collision collision)
    {
        if(state == SpiritState.Flight)
        {
            var enemy = collision.collider.GetComponent<Enemy>();

            if(enemy)
            {
                // TODO: Enemy take damage
                Explode();
            } else if(flightForce >= explosionLimit)
            {
                Explode();
            }
        }
    }
}
