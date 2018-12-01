using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Attack Scene")]
    public Vector3 asLookTargetOffset = new Vector3(0, 1, 0);
    public Vector3 asCameraDelta = new Vector3(1, 1, -5);
    public float asSlowScale = 0.2f, asCameraMoveTime = 0.5f,
        asUpdateTime = 0.02f, asPauseTime = 1.0f;

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
        CameraBounds = FindObjectOfType<CameraBounds>();
    }

    public void SetState(GameState state)
    {
        State = state;
    }
    
    private void Update ()
    {
        if (State != GameState.Idle)
            return;

        player.HandleUpdate();
        gameCamera.HandleUpdate();
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

    public void InitAttackScene(Spirit targetSpirit)
    {
        StartCoroutine(_PlayAttackScene(targetSpirit));
    }

    private IEnumerator _PlayAttackScene(Spirit targetSpirit)
    {
        yield return null;
        // Set state to pause other game logic
        SetState(GameState.Event);

        var cameraTransform = gameCamera.Camera.transform;
        var originalCameraPos = cameraTransform.position;
        var originalCameraRot = cameraTransform.rotation;
        var cameraTargetPos = player.transform.position + player.transform.rotation * asCameraDelta;

        Time.timeScale = asSlowScale;
        yield return _MoveCamera(cameraTransform, cameraTargetPos, targetSpirit.transform);
        Time.timeScale = 1.0f;

        yield return _ReturnCamera(cameraTransform, originalCameraPos, originalCameraRot);

        // Set state back
        SetState(GameState.Idle);
    }

    private IEnumerator _MoveCamera(Transform cameraTransform, Vector3 targetPos, Transform targetSpiritTransform)
    {
        var t = 0.0f;
        var startPos = cameraTransform.position;

        while(t < asCameraMoveTime)
        {
            t += asUpdateTime;
            cameraTransform.position = Vector3.Lerp(startPos, targetPos, t / asCameraMoveTime);

            var lookTargetPos = (player.transform.position + targetSpiritTransform.position) / 2
            + asLookTargetOffset;
            cameraTransform.LookAt(lookTargetPos);
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
            yield return new WaitForSecondsRealtime(asUpdateTime);
        }
    }
}
