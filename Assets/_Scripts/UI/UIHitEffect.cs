using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHitEffect : MonoBehaviour {

    public AnimationCurve scaleCurve;
    public float time = 0.3f;
    
	void Start () {
        transform.localScale = Vector3.zero;
        StartCoroutine(Animate());
	}
	
    private IEnumerator Animate()
    {
        var t = 0.0f;
        while (t < time)
        {
            t += GameManager.Instance.asUpdateTime;
            transform.localScale = Vector3.one * scaleCurve.Evaluate(t / time);
            yield return new WaitForSecondsRealtime(GameManager.Instance.asUpdateTime);
        }
        Destroy(gameObject);
    }
}
