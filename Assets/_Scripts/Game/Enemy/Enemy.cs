﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    protected float visionRange = 10;

    [SerializeField]
    protected float maxActionDelay = 1.0f;  // The time it takes for the enemy to take a new action [0, maxAttackDelay]
    protected float actionDelay;

    [SerializeField]
    private bool hasAction = true;

    public float attackRange = 0.6f;
    public float wanderTime = 1.0f, attackTime = 0.5f, maxMoveTime = 3.0f, speed;
    public bool attacking = false;

    [Header("Stats")]
    public int hp = 100;
    public int strength = 10, killExp = 50, soulStrength = 8, level = 1;
    public UIEnemyIcon icon;
    public UIStatBlock statBlock;
    public Vector3 statBlockMargin = Vector3.up * 2;
    public AudioSource hitSound;

    protected EnemyCollider[] colliders;
    protected Rigidbody rBody;
    protected Animator animator;
    protected int startHp;

    public float HealthRatio { get { return (float)hp / startHp; } }

    protected virtual void Start()
    {
        colliders = GetComponentsInChildren<EnemyCollider>();
        rBody = GetComponent<Rigidbody>();
        Init();

        startHp = hp;
        // Spawn icon
        icon = Instantiate(icon, GameManager.Instance.uiCanvas.iconContainer);
        icon.SetOrientation(GameManager.Instance.player.transform.position, transform.position);
        statBlock = Instantiate(statBlock, GameManager.Instance.uiCanvas.iconContainer);
        statBlock.SetValues(transform.position, HealthRatio, statBlockMargin, level);

        // Add to list
        GameManager.Instance.enemies.Add(this);
    }

    public void SetAnimatorSpeed(float speed)
    {
        if (animator)
            animator.speed = speed;
    } 

    protected virtual void Init()
    {
        actionDelay = Random.Range(maxActionDelay / 2, maxActionDelay);
        hasAction = attacking = false;

        for (int i = 0; i < colliders.Length; i++)
            colliders[i].ResetCollider();
    }

    protected virtual void StopAction()
    {
        Init();
    }

    protected virtual void FixedUpdate()
    {
        if (hasAction || !GameManager.Instance.IsIdle)
            return;

        actionDelay -= Time.fixedDeltaTime;
        if (actionDelay <= 0)
        {
            StartCoroutine(DoAction());
        }
    }

    protected virtual void LateUpdate()
    {
        icon.SetOrientation(GameManager.Instance.player.transform.position, transform.position);
        statBlock.SetValues(transform.position, HealthRatio, statBlockMargin);
    }

    protected virtual IEnumerator DoAction()
    {
        if (GameManager.Instance.spirits.Count == 0)
            yield break;

        hasAction = true;
        yield return MoveToTarget(attackRange);
        
        attacking = true;
        yield return Attack();
        attacking = false;
        Init();
    }

    protected virtual IEnumerator MoveToTarget(float range)
    {
        // Find target spirit
        var spirits = GameManager.Instance.spirits;
        var count = spirits.Count;

        if (count == 0)
            yield break;

        Spirit target = null;
        for(int i = 0; i < count; i++)
        {
            if(Vector3.SqrMagnitude(transform.position - spirits[i].transform.position)
                < visionRange)
            {
                if(!target || Random.Range(0f, 1f) < 0.5f)
                    target = spirits[i];
            }
        }

        if(!target)
        {
            // Find target with the lowest soul
            target = spirits[0];
            for (int i = 1; i < count; i++)
            {
                if (spirits[i].soul < target.soul)
                {
                    target = spirits[i];
                    break;
                }
            }
        }
        
        // Move
        var sqrDist = range * range;
        transform.LookAt(target.transform);
        var t = 0f;
        while (Vector3.SqrMagnitude(transform.position - target.transform.position) > sqrDist
            && t < maxMoveTime)
        {
            t += Time.fixedDeltaTime;
            MoveTowards(target.transform.position);
            yield return new WaitForFixedUpdate();
        }
    }

    protected void MoveTowards(Vector3 position)
    {
        if (!rBody)
            return;

        transform.LookAt(position);

        rBody.velocity = Vector3.zero;
        rBody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }

    protected IEnumerator Attack()
    {
        // Rotation attack
        var t = 0.0f;
        while (t < attackTime)
        {
            if (GameManager.Instance.IsIdle)
            {
                t += Time.fixedDeltaTime;
                AttackFrame(t);
            } else
            {
                rBody.velocity = Vector3.zero;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    protected virtual void AttackFrame(float t)
    {
        var delta = Mathf.Min(Time.fixedDeltaTime / attackTime, attackTime - t);
        transform.Rotate(new Vector3(0, delta * 360, 0));
    }
    
    public void TakeDamage(Spirit source, int amount)
    {
        if (GameManager.Instance.end)
            return;

        var damage = Mathf.Min(hp, amount);
        hp -= damage;

        if (hp <= 0)
        {
            OnDeath(source);
        }
        else if (source)
        {
            source.AddExp(damage);
            var dir = transform.position - source.transform.position;
            rBody.AddForce(dir.normalized, ForceMode.VelocityChange);
        }
    }
    

    protected virtual void OnDeath(Spirit source)
    {
        // remove from list
        GameManager.Instance.enemies.Remove(this);

        if(source)
            source.AddExp(killExp);
        Destroy(gameObject);
        Destroy(icon.gameObject);
        Destroy(statBlock.gameObject);
    }
}
