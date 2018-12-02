using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour {

    public string nextScene;
    public AudioSource sound;
    public float soundDelay = 0.1f, loadDelay = 2.0f;

    private void Start()
    {
        StartCoroutine(Play());
    }

	private IEnumerator Play()
    {
        yield return new WaitForSeconds(soundDelay);
        sound.Play();
        yield return new WaitForSeconds(loadDelay);
        LoadingScreen.Instance.LoadScene(nextScene);
    }
}
