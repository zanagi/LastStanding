using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIntro : MonoBehaviour {

    public Transform[] targets;
    public float delay = 1.0f, targetTime = 0.5f, targetPauseTime = 0.5f,
        time1 = 1.0f, pTime1 = 2.0f, time2 = 1.0f;
    public MaskableGraphic introObject;

    protected Transform cameraTransform, playerTransform;
    protected Vector3 offset;

	// Use this for initialization
	void Start ()
    {
        GameManager.Instance.SetState(GameState.Event);
        cameraTransform = GameManager.Instance.gameCamera.Camera.transform;
        playerTransform = GameManager.Instance.player.transform;
        offset = cameraTransform.position - playerTransform.position;

        Static.SetAlpha(introObject, 0.0f);
        introObject.gameObject.SetActive(true);

        StartCoroutine(Play());
	}
	
	protected virtual IEnumerator Play()
    {
        yield return new WaitForSeconds(delay);
        yield return FadeIntro(true);
        yield return new WaitForSeconds(pTime1);
        yield return FadeIntro(false);
        yield return LoopTargets();
        GameManager.Instance.SetState(GameState.Idle);
    }
    
    private IEnumerator FadeIntro(bool fadeIn)
    {
        var t = 0.0f;

        while(t < time1)
        {
            t += Time.deltaTime;
            var ratio = t / time1;
            Static.SetAlpha(introObject, fadeIn ? ratio : 1.0f - ratio);
            yield return null;
        }
    }

    protected IEnumerator LoopTargets()
    {
        for(int i = 0; i < targets.Length; i++)
        {
            yield return AnimateMove(cameraTransform, targets[i].transform.position + offset, targetTime);
            yield return new WaitForSeconds(targetPauseTime);
        }
        yield return AnimateMove(cameraTransform, playerTransform.position + offset, targetTime);
    }

    protected IEnumerator AnimateMove(Transform transform, Vector3 endPos, float time)
    {
        var startPos = transform.position;
        var t = 0f;
        while(t < time)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t / time);
            yield return null;
        }
    }

}
