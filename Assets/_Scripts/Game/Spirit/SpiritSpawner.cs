﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpiritSpawner : MonoBehaviour {

    [SerializeField] protected int count = 10;
    [SerializeField] private Spirit[] spiritPrefabs;
    [SerializeField] private Color[] baseColors;
    [SerializeField] private Color[] rimColors;

    protected virtual void Start()
    {
        SpawnSpirits();
    }
    
    public void SpawnSpirit(Vector3 pos)
    {
        var spirit = Instantiate(spiritPrefabs.GetRandom());
        spirit.transform.position = pos;
        spirit.SetColors(baseColors.GetRandom(), rimColors.GetRandom());
    }

    public abstract void SpawnSpirits();
}