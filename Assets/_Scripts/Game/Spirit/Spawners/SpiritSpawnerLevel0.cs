using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritSpawnerLevel0 : SpiritSpawner {
    
    [SerializeField] private int xCount, yCount;
    [SerializeField] private float xMargin, yMargin;

    public override void SpawnSpirits()
    {
        var center = GameManager.Instance.player.transform.position;

        for (int x = -xCount / 2; x <= xCount / 2; x++)
        {
            for(int y = -yCount / 2; y <= yCount / 2; y++)
            {
                if(x != 0 || y != 0)
                    SpawnSpirit(center + new Vector3(x * xMargin, 0, y * yMargin));
            }
        }
    }
}
