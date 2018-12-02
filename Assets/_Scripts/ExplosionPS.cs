using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPS : MonoBehaviour {

    public int explosionDamage = 30;
    public AnimationCurve scaleCurve;
    public bool hasParent = true;
    private ParticleSystem ps;
    private float duration, time;

    private List<Enemy> enemiesHit = new List<Enemy>();

	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
        duration = ps.main.duration;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time / duration);

        if(time >= duration)
        {
            Destroy(hasParent ? transform.parent.gameObject : gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponentInParent<Enemy>();

        if(enemy && !enemiesHit.Contains(enemy))
        {
            enemy.TakeDamage(null, explosionDamage);
            enemiesHit.Add(enemy);
        }
    }
}
