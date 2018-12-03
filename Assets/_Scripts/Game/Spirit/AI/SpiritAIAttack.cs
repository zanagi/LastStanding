using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritAIAttack : SpiritAI {


    [SerializeField]
    protected float visionRange = 10;

    [SerializeField]
    protected float maxActionDelay = 1.0f;  // The time it takes for the enemy to take a new action [0, maxAttackDelay]
    protected float actionDelay;

    [SerializeField]
    private bool hasAction = true;

    public float attackRange = 0.3f;
    public float wanderTime = 1.0f, attackTime = 0.5f, maxMoveTime = 3.0f;
    public bool attacking = false;
    public string attackState = "Attack", idleState = "Idle", runState = "Run";
    private Spirit spirit;

    protected override void Start()
    {
        base.Start();
        Init();
    }

    protected virtual void Init()
    {
        actionDelay = Random.Range(maxActionDelay / 2, maxActionDelay);
        hasAction = attacking = false;
    }

    protected virtual void StopAction()
    {
        Init();
    }

    public override void HandleUpdate(Spirit spirit)
    {
        if (hasAction || !GameManager.Instance.IsIdle)
            return;

        if (!this.spirit)
            this.spirit = spirit;

        actionDelay -= Time.deltaTime;
        if (actionDelay <= 0)
        {
            StartCoroutine(DoAction(spirit));
        }
    }


    protected virtual IEnumerator DoAction(Spirit spirit)
    {
        if (GameManager.Instance.enemies.Count == 0)
            yield break;

        hasAction = true;
        yield return MoveToTarget(attackRange, spirit);

        attacking = true;
        yield return Attack();
        attacking = false;
        Init();
    }


    protected virtual IEnumerator MoveToTarget(float range, Spirit spirit)
    {
        var enemies = GameManager.Instance.enemies;
        var count = enemies.Count;

        if (count == 0)
            yield break;

        Enemy target = null;
        for (int i = 0; i < count; i++)
        {
            if (Vector3.SqrMagnitude(transform.position - enemies[i].transform.position)
                < visionRange)
            {
                if (!target || Static.TransformCloser(transform, enemies[i].transform, target.transform))
                    target = enemies[i];
            }
        }

        if(!target)
            target = enemies[0];

        // Move
        animator.CrossFade(runState, 0.5f);
        var sqrDist = range * range;
        transform.LookAt(target.transform);
        var t = 0f;
        while (target && Vector3.SqrMagnitude(transform.position - target.transform.position) > sqrDist)
        {
            t += Time.fixedDeltaTime;
            MoveTowards(target.transform.position, spirit);
            yield return new WaitForFixedUpdate();
        }
    }

    protected void MoveTowards(Vector3 position, Spirit spirit)
    {
        if (!spirit.rBody)
            return;

        transform.LookAt(position);
        spirit.rBody.velocity = Vector3.zero;
        spirit.rBody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }


    protected IEnumerator Attack()
    {
        animator.CrossFade(attackState, 0.1f);

        while(attacking)
        {
            yield return null;
        }
        animator.CrossFade(idleState, 0.1f);
    }

    public void SetAttackState()
    {
        spirit.state = SpiritState.Attack;
    }

    public void StopAttack()
    {
        attacking = false;
        spirit.state = SpiritState.Idle;
    }

    public void PlayFootstep()
    {

    }
}
