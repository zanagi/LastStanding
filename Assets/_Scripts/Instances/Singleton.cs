using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : BaseSingleton where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    public override void AssignInstance()
    {
        Instance = GetComponent<T>();
    }

    private void Awake()
    {
        if (Instance && Instance != GetComponent<T>())
        {
            Destroy(this);
            return;
        }
        Init();
    }

    protected virtual void Init()
    {
        // Init code to override
    }
}
