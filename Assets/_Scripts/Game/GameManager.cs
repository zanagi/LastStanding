using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Idle,
    Event,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; }
    private Player player;

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
	}

    private void FixedUpdate()
    {
        if (State != GameState.Idle)
            return;

        player.HandleFixedUpdate();
    }
}
