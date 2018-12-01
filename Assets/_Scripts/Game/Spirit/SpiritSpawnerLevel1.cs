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
            float x = minRadius + Random.Range(0, maxRadius - minRadius);
            float z = minRadius + Random.Range(0, maxRadius - minRadius);

            if (Random.Range(0f, 1f) < 0.5f)
                x *= -1;
            if (Random.Range(0f, 1f) < 0.5f)
                z *= -1;
            SpawnSpirit(new Vector3(x, 0, z));
        }
    }
}
