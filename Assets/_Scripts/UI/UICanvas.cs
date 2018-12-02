using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour {

    public Transform iconContainer;
    public GameObject hitSoundPrefab, enemyHitSoundPrefab;
    public UIExplosionEffect explosionPrefab;

    public void SpawnHitSound(Vector3 pos, bool byEnemy)
    {
        var temp = Instantiate(byEnemy ? enemyHitSoundPrefab : hitSoundPrefab, transform);
        temp.transform.position = pos;
    }

    public void SpawnExplosionSound(Vector3 worldPos)
    {
        var temp = Instantiate(explosionPrefab, transform);
        temp.sourcePos = worldPos;
    }
}
