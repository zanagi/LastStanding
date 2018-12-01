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
    [HideInInspector] public Player player;
    [HideInInspector] public GameCamera gameCamera;

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
}
