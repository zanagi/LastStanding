using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A static manager that parents and controls all static instances/singletons
public class InstanceManager : MonoBehaviour {

    private static InstanceManager instance;

    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Load and assign all instances
        var singletons = GetComponentsInChildren<BaseSingleton>(true);
        for (int i = 0; i < singletons.Length; i++)
            singletons[i].AssignInstance();
    }
}
