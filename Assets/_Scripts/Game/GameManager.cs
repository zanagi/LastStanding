﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Idle,
    Event,
}

// An instance for managing main game behavior (player & camera)
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; }
    public bool IsIdle { get { return State == GameState.Idle; } }
    public CameraBounds CameraBounds { get; private set; }

    [HideInInspector] public Player player;
    [HideInInspector] public GameCamera gameCamera;
    [HideInInspector] public UICanvas uiCanvas;

    [Header("Attack Scene")]
    public Vector3 asLookTargetOffset = new Vector3(0, 1, 0);
    public Vector3[] asCameraDelta;
    public float asSlowScale = 0.2f, asCameraMoveTime = 0.5f,
        asUpdateTime = 0.02f, asPauseTime = 1.0f;
    public Image asFocusImage;
    public AudioSource asFocusSource;

    [Header("Explosion")]
    public GameObject explosionPrefab;

    // Other list
    [HideInInspector] public List<Spirit> spirits = new List<Spirit>();
    [HideInInspector] public List<Enemy> enemies = new List<Enemy>();

    [Header("End")]
    public GameObject gameOverObject;
    public bool end;

    [HideInInspector] public SceneSwitcher switcher;

    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Load variables
        player = GetComponentInChildren<Player>();
        gameCamera = GetComponentInChildren<GameCamera>();
        uiCanvas = GetComponentInChildren<UICanvas>();
        CameraBounds = FindObjectOfType<CameraBounds>();
        switcher = GetComponent<SceneSwitcher>();
        gameOverObject.SetActive(false);
        
        // Attack scene focus
        asFocusImage.gameObject.SetActive(false);
        Static.SetAlpha(asFocusImage, 0.0f);
    }

    public void SetState(GameState state)
    {
        State = state;
    }
    
    private void Update ()
    {
        // Check end
        if (enemies.Count <= 0 && !switcher.IsActive)
        {
            switcher.SwitchScene();
            return;
        }

        if (State != GameState.Idle)
            return;

        player.HandleUpdate();
        gameCamera.HandleUpdate();
    
        for(int i = 0; i < spirits.Count; i++)
        {
            spirits[i].HandleAiUpdate();
        }
	}

    private void FixedUpdate()
    {
        if (State != GameState.Idle)
            return;

        player.HandleFixedUpdate();
        gameCamera.HandleFixedUpdate();
    }

    private void LateUpdate()
    {
        if (State != GameState.Idle)
            return;

        player.HandleLateUpdate();
    }

    public Vector3 ScreenPos(Vector3 worldPos)
    {
        return gameCamera.Camera.WorldToScreenPoint(worldPos);
    }

    public void SpawnExplosion(Vector3 pos)
    {
        uiCanvas.SpawnExplosionSound(pos);
        Instantiate(explosionPrefab, pos, Quaternion.identity);
    } 

    public void GameOver()
    {
        end = true;
        State = GameState.Event;
        gameOverObject.SetActive(true);
    }

    public void Retry()
    {
        LoadingScreen.Instance.ReloadScene();
    }

    public void BackToMenu()
    {
        LoadingScreen.Instance.LoadScene("Menu");
    }

    public void InitAttackScene(Spirit targetSpirit)
    {
        StartCoroutine(_PlayAttackScene(targetSpirit));
    }

    private IEnumerator _PlayAttackScene(Spirit targetSpirit)
    {
        yield return null;

        // Set state to pause other game logic
        SetState(GameState.Event);
        SetAnimators(false);

        // Play audio
        asFocusSource.Play();

        var cameraTransform = gameCamera.Camera.transform;
        var originalCameraPos = cameraTransform.position;
        var originalCameraRot = cameraTransform.rotation;
        var cameraTargetPos = player.transform.position + player.transform.rotation * asCameraDelta.GetRandom();

        Time.timeScale = asSlowScale;
        yield return _MoveCamera(cameraTransform, cameraTargetPos, targetSpirit.transform);
        Time.timeScale = 1.0f;

        yield return _ReturnCamera(cameraTransform, originalCameraPos, originalCameraRot);

        // Set state back
        SetState(GameState.Idle);
        SetAnimators(true);
    }

    private void SetAnimators(bool active)
    {
        for(int i = 0; i < spirits.Count; i++)
        {
            if (spirits[i].ai)
            {
                spirits[i].ai.animator.speed = active ? 1 : 0;
                spirits[i].rBody.velocity = Vector3.zero;
            }
        }
    }

    private IEnumerator _MoveCamera(Transform cameraTransform, Vector3 targetPos, Transform targetSpiritTransform)
    {
        asFocusImage.gameObject.SetActive(true);
        var t = 0.0f;
        var startPos = cameraTransform.position;

        while(t < asCameraMoveTime)
        {
            t += asUpdateTime;
            var ratio = t / asCameraMoveTime;
            cameraTransform.position = Vector3.Lerp(startPos, targetPos, ratio);

            var lookTargetPos = (player.transform.position + targetSpiritTransform.position) / 2
            + asLookTargetOffset;
            cameraTransform.LookAt(lookTargetPos);
            asFocusImage.SetAlpha(ratio);
            yield return new WaitForSecondsRealtime(asUpdateTime);
        }

        t = 0.0f;
        while(t < asPauseTime)
        {
            t += asUpdateTime;

            var lookTargetPos = (player.transform.position + targetSpiritTransform.position) / 2
            + asLookTargetOffset;
            cameraTransform.LookAt(lookTargetPos);
            yield return new WaitForSecondsRealtime(asUpdateTime);
        }
    }

    private IEnumerator _ReturnCamera(Transform cameraTransform, Vector3 cameraPos, Quaternion cameraRot)
    {
        var t = 0.0f;
        var startPos = cameraTransform.position;
        var startRot = cameraTransform.rotation;

        while (t < asCameraMoveTime)
        {
            t += asUpdateTime;
            var ratio = t / asCameraMoveTime;
            cameraTransform.position = Vector3.Lerp(startPos, cameraPos, ratio);
            cameraTransform.rotation = Quaternion.Lerp(startRot, cameraRot, ratio);
            asFocusImage.SetAlpha(1.0f - ratio);
            yield return new WaitForSecondsRealtime(asUpdateTime);
        }
        asFocusImage.gameObject.SetActive(false);
    }
}
