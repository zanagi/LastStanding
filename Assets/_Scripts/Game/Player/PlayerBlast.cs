using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlast : MonoBehaviour {

    public float launchTime, force = 8;
    public int damage = 50, maxDist = 50, cost = 12;
    private bool launched;

	// Use this for initialization
	void Start ()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(Play());
	}
	
	private IEnumerator Play()
    {
        var t = 0.0f;

        while(t < launchTime)
        {
            t += Time.deltaTime;
            transform.localScale = Mathf.Min(1, t / launchTime) * Vector3.one;
            yield return null;
        }

        launched = true;

        var dist = 0f;
        var dir = transform.forward;
        while(dist < maxDist)
        {
            dist += Time.fixedDeltaTime * force;
            transform.position += dir * dist;
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(launched)
        {
            var enemy = col.GetComponentInParent<Enemy>();

            if (enemy)
                enemy.TakeDamage(null, damage);
            GameManager.Instance.uiCanvas.SpawnExplosionSound(transform.position);
            Destroy(gameObject);
        }
    }
}
