using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour {

    public string nextScene;
    public float delay = 1.0f, time1 = 1.0f, pTime1 = 2.0f, time2 = 1.0f;
    public MaskableGraphic introObject;
    
    public bool IsActive { get; private set; }

    void Start ()
    {
        Static.SetAlpha(introObject, 0.0f);
        introObject.gameObject.SetActive(true);
    }

    public void SwitchScene()
    {
        IsActive = true;
        GameManager.Instance.SetState(GameState.Event);
        StartCoroutine(Play());
    }

    protected virtual IEnumerator Play()
    {
        yield return new WaitForSeconds(delay);
        yield return FadeObject(true);
        yield return new WaitForSeconds(pTime1);
        LoadingScreen.Instance.LoadScene(nextScene);
    }

    private IEnumerator FadeObject(bool fadeIn)
    {
        var t = 0.0f;

        while (t < time1)
        {
            t += Time.deltaTime;
            var ratio = t / time1;
            Static.SetAlpha(introObject, fadeIn ? ratio : 1.0f - ratio);
            yield return null;
        }
    }
}
