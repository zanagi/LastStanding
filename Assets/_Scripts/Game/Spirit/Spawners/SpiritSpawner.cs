using System.Collections;
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

    public void SpawnSpirit(Vector3 pos, bool lookAtPlayer = false, bool reverse = false)
    {
        var spirit = Instantiate(spiritPrefabs.GetRandom());
        spirit.transform.position = pos;
        spirit.SetColors(baseColors.GetRandom(), rimColors.GetRandom());

        if (lookAtPlayer)
            spirit.transform.LookAt(GameManager.Instance.player.transform);
        if (reverse)
            spirit.transform.Rotate(0, 180, 0);
    }

    public abstract void SpawnSpirits();
}
