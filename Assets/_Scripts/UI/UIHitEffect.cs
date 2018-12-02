using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHitEffect : MonoBehaviour {

    public AnimationCurve scaleCurve;
    public float time = 0.3f;
    private float currentTime;
    
	void Start () {
        transform.localScale = Vector3.zero;
	}
	
	void Update ()
    {
        if (currentTime >= time)
            return;

        currentTime += Time.deltaTime;
        transform.localScale = Vector3.one * scaleCurve.Evaluate(currentTime / time);

        if (currentTime >= time)
            Destroy(gameObject);
	}
}
