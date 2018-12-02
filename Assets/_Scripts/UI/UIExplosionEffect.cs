using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExplosionEffect : UIHitEffect {

    public Vector3 sourcePos, margin = Vector3.up * 0.5f;

	void Update ()
    {
        transform.position = GameManager.Instance.gameCamera.Camera.WorldToScreenPoint(sourcePos + margin);
	}
}
