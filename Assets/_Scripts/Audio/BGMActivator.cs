using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMActivator : MonoBehaviour {
    
    [SerializeField]
    private bool instant = false;
    
    // Use this for initialization
    void Start()
    {
        var audio = GetComponent<AudioSource>();
        if (audio)
            BGMManager.Instance.SetBGM(audio, instant);
    }
}
