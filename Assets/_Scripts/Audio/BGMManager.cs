using System.Collections;
using UnityEngine;

public class BGMManager : Singleton<BGMManager> {
    
    [SerializeField]
    private float bgmChangeTime = 3.0f;

    [SerializeField]
    private AudioSource[] bgmPrefabs;
    private AudioSource currentSource;
    private bool settingBGM;
    
    public void SetBGM(AudioSource bgmPrefab, bool instant = false)
    {
        StartCoroutine(SetBGMEnum(bgmPrefab, instant));
    }

    public void SetBGM(string name, bool instant = false)
    {
        for (int i = 0; i < bgmPrefabs.Length; i++)
        {
            if (bgmPrefabs[i].name == name)
            {
                SetBGM(bgmPrefabs[i], instant);
                break;
            }
        }
    }

    public void StopBGM()
    {
        StartCoroutine(StopCurrentBGM());
    }

    private IEnumerator SetBGMEnum(AudioSource bgmObject, bool instant = false)
    {
        while (settingBGM)
            yield return null;

        // Check same bgm
        if (currentSource == bgmObject)
            yield break;

        settingBGM = true;
        if (!instant)
            yield return StopCurrentBGM();

        if (currentSource)
            Destroy(currentSource);

        currentSource = bgmObject;
        currentSource.transform.SetParent(transform);
        currentSource.Play();

        if (!instant)
            yield return FadeInCurrentBGM();
        settingBGM = false;
    }

    private IEnumerator StopCurrentBGM()
    {
        if (currentSource)
        {
            while (currentSource.volume > 0.0f)
            {
                currentSource.volume -= Time.fixedDeltaTime / bgmChangeTime;
                yield return new WaitForFixedUpdate();
            }
            currentSource.volume = 0.0f;
            Destroy(currentSource);
        }
    }

    private IEnumerator FadeInCurrentBGM()
    {
        var targetVolume = currentSource.volume;
        currentSource.volume = 0.0f;

        while (currentSource.volume < targetVolume)
        {
            currentSource.volume = Mathf.Min(targetVolume, currentSource.volume + Time.fixedDeltaTime / bgmChangeTime);
            yield return new WaitForFixedUpdate();
        }
        currentSource.volume = targetVolume;
    }
}
