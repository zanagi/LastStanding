using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPS : MonoBehaviour {

    public int explosionDamage = 30;
    public AnimationCurve scaleCurve;
    public bool hasParent = true;
    private ParticleSystem ps;
    private float duration, time;

	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
        duration = ps.main.duration;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time / duration);
        Debug.Log(transform.localScale);

        if(time >= duration)
        {
            Destroy(hasParent ? transform.parent.gameObject : gameObject);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponentInParent<Enemy>();

        if(enemy)
        {
            enemy.TakeDamage(null, explosionDamage);
        }
    }
}
