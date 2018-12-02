using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritSpawnerLevel4 : SpiritSpawner {

    [SerializeField] private float r = 8.0f, angleOffset = 30f;

    public override void SpawnSpirits()
    {
        for(int i = 0; i < count; i++)
        {
            var angle = i * 360f / count + angleOffset;
            SpawnSpirit(new Vector3(r * Mathf.Cos(angle), 0, r * Mathf.Sin(angle)), true);
        }
    }
}
