using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritSpawnerLevel1 : SpiritSpawner {

    [SerializeField] private float minRadius = 4.0f;
    [SerializeField] private float maxRadius = 13.0f;

    public override void SpawnSpirits()
    {
        for(int i = 0; i < count; i++)
        {
            var r = Random.Range(minRadius, maxRadius);
            var angle = Random.Range(0f, 360f);
            SpawnSpirit(new Vector3(r * Mathf.Cos(angle), 0, r * Mathf.Sin(angle)));
        }
    }
}
