using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spirit : MonoBehaviour {

    private static int levelExp = 100;


    [Header("Level")]
    public int level = 1;
    private int exp = 0;

    [Header("Class specific values")]
    public int health = 100;
    public int soul = 100;
    private int maxHealth = 100, maxSoul = 100;
    public float soulModifier = 1.0f;
    public float attackForce = 4.0f;
    public int strength = 5, bombStrength;

    [Header("Audio")]
    public AudioSource explosionSource;
    public AudioSource hitSource;

    [HideInInspector] public SpiritState state;
    [HideInInspector] public Rigidbody rBody;

    // Received attack from other
    [HideInInspector] public Spirit attacker;
    [HideInInspector] public float flightForce;
    [HideInInspector] public Vector3 flightDirection;
    private bool flightCheck = false;
    private float flightFriction = 40.0f, flightMass = 40.0f, normalMass,
        explosionLimit = 4;

    // Glow/Shader variables
    private static string soulEffectName = "_RimPower", baseColorName = "_BaseColor", glowColorName = "_RimColor";
    private static float maxSoulEffect = 1, minSoulEffect = 4, flightMinSqr = 0.01f;
    private Material[] materials;
    private int previousSoul;

    // AI
    [HideInInspector] public SpiritAI ai;
    [HideInInspector] public Animator animator;
    public UnityEvent onDeath;
    private bool isDead;

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

        maxHealth = health;
        maxSoul = soul;
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

        // AI
        ai = GetComponent<SpiritAI>();
        animator = GetComponent<Animator>();

        // Add to list
        GameManager.Instance.spirits.Add(this);
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
        // Flight hass priority
        CheckFlight();

        if (!GameManager.Instance.IsIdle)
            return;

        CheckSoul();
        CheckHealth();
    }

    public void HandleAiUpdate()
    {
        if (ai)
            ai.HandleUpdate(this);
    }
    
    private void CheckHealth()
    {
        if(health <= 0 && !isDead)
        {
            Explode();
        }
    }

    public void TakeDamage(Enemy enemy)
    {
        if (GameManager.Instance.end)
            return;

        var damage = enemy.strength;
        var r = SoulRatio;

        if(r >= 0.8f)
        {
            damage /= 4;
        } else if(r >= 0.6)
        {
            damage /= 2;
        }
        else if (r >= 0.2f)
        {
            damage = (int)(damage * 1.5f);
        } else if(r > 0)
        {
            damage *= 2;
        } else
        {
            damage *= 3;
        }
        health = Mathf.Max(0, health - damage);
        OnHit(enemy.transform.position, true);
    }

    private void OnHit(Vector3 cause, bool byEnemy)
    {
        // Audio
        hitSource.Play();

        var camera = GameManager.Instance.gameCamera.Camera;
        var hitPos = camera.WorldToScreenPoint((cause + transform.position) / 2 + 0.5f * Vector3.up);       
        GameManager.Instance.uiCanvas.SpawnHitSound(hitPos, byEnemy);
    }

    private void CheckSoul()
    {
        /*
        if (previousSoul != soul)
            SetGlow();
            */
        previousSoul = soul;
    }
    
    private void CheckFlight()
    {
        if (state == SpiritState.Flight)
        {
            if (flightForce <= 0 || (flightCheck && rBody.velocity.sqrMagnitude < flightMinSqr))
            {
                StopFlight();
                if (flightForce >= explosionLimit)
                    Explode();
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

        // Soul reduction
        if (soul < source.soul)
            source.ReduceSoul(4);
        else if (soul == source.soul)
            source.ReduceSoul(8);
        else
            source.ReduceSoul(12);

        OnHit(source.transform.position, false);
    }

    public void Explode()
    {
        if (explosionSource)
        {
            explosionSource.transform.SetParent(null);
            explosionSource.Play();
            explosionSource.gameObject.AddComponent<AutoDestroyAS>();
        }
        GameManager.Instance.SpawnExplosion(transform.position);
        Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        isDead = true;
        onDeath.Invoke();

        // Remove from list
        GameManager.Instance.spirits.Remove(this);
    }

    private int collisionCount = 0;

    // Check flight explosion
    private void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.collider.GetComponentInParent<Enemy>();

        if (state == SpiritState.Flight)
        {
            if(enemy)
            {
                enemy.TakeDamage(this, bombStrength);
                Explode();
            } else if(flightForce >= explosionLimit || collisionCount > 0)
            {
                Explode();
            }
        }
    }
}
