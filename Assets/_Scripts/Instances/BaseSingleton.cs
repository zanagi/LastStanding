using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class inherited by all singletons
public abstract class BaseSingleton : MonoBehaviour
{
    public abstract void AssignInstance();
}
