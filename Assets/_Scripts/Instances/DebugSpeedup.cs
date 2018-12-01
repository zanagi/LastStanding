using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSpeedup : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.0f;

    [SerializeField]
    private KeyCode keyCode = KeyCode.Tab;

    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            Time.timeScale = speed;
        }
        else if (Input.GetKeyUp(keyCode))
        {
            Time.timeScale = 1.0f;
        }
    }
}
