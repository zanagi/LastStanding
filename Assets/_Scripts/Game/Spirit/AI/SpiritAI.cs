using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritAI : MonoBehaviour {

    public float speed = 1;
    [HideInInspector] public Animator animator;
    protected static string startState = "Idle";
    
	protected virtual void Start ()
    {
        // Randomize animation time
        animator = GetComponent<Animator>();
        animator.Play(startState, 0, Random.Range(0f, 1f));
	}

    public virtual void HandleUpdate(Spirit spirit)
    {

    }

    protected Enemy GetClosestEnemy()
    {
        var enemies = GameManager.Instance.enemies;

        if (enemies.Count == 0)
            return null;
        var enemy = enemies[0];

        for(int i = 1; i < enemies.Count; i++)
        {
            if(Static.TransformCloser(transform, enemies[i].transform, enemy.transform))
            {
                enemy = enemies[i];
            }
        }
        return enemy;
    }
}
