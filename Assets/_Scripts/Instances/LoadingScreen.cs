using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : Singleton<LoadingScreen> {

    [SerializeField] private Image background;
    [SerializeField] private float animationTime;

    public bool IsLoading { get; private set; }
    public string CurrentScene { get; private set; }

    protected override void Init()
    {
        CurrentScene = SceneManager.GetActiveScene().name;

        // Hide background on load
        background.SetAlpha(0.0f);
        background.gameObject.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        if (IsLoading)
            return;
        StartCoroutine(AnimateLoad(sceneName));
    }

    private IEnumerator AnimateLoad(string sceneName)
    {
        IsLoading = true;

        // Animate fade-in
        yield return AnimateBackgroundFade(1.0f);
        
        // Load scene
        var loadOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!loadOperation.isDone)
            yield return null;
        
        // Animate fade-out
        yield return AnimateBackgroundFade(0.0f);
        CurrentScene = sceneName;
        IsLoading = false;
    }
    
    public IEnumerator AnimateBackgroundFade(float target)
    {
        background.gameObject.SetActive(true);

        var t = 0.0f;
        var currentAlpha = background.color.a;
        while (t < animationTime)
        {
            t += Time.deltaTime;
            background.SetAlpha(Mathf.Lerp(currentAlpha, target, t / animationTime));
            yield return null;
        }
        background.gameObject.SetActive(target > 0);
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
}
