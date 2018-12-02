using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyAS : MonoBehaviour {

    private AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(source && !source.isPlaying && !transform.parent)
        {
            Destroy(gameObject);
        }
	}
}
