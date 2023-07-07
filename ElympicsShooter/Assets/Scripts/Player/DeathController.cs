using Elympics;
using System;
using UnityEngine;

public class DeathController : ElympicsMonoBehaviour, IUpdatable, IInitializable
{
    [Header("Parameters:")]
    [SerializeField] private float deathTime = 2.0f;

    [Header("References:")]
    [SerializeField] private GameStateController gameStateController = null;
    [SerializeField] private Collider[] playerColliders = null;
    [SerializeField] private Rigidbody playerRigidbody = null;

    public ElympicsBool IsDead { get; } = new ElympicsBool(false);
    public ElympicsFloat CurrentDeathTime { get; } = new ElympicsFloat(0.0f);
    public ElympicsInt KillerId { get; } = new ElympicsInt(-1);

    public event Action PlayerRespawned = null;
    public event Action<int, int> HasBeenKilled = null;


    private PlayerData playerData = null;

    public void Initialize()
    {
        playerData = GetComponent<PlayerData>();
        IsDead.ValueChanged += HandleAfterlifePhysics;
    }

    private void HandleAfterlifePhysics(bool lastValue, bool newValue)
    {
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.useGravity = !newValue;

        foreach (var collider in playerColliders)
        {
            collider.enabled = !newValue;
        }
    }

    public void ProcessPlayersDeath(int damageOwner)
    {
        CurrentDeathTime.Value = deathTime;
        IsDead.Value = true;
        KillerId.Value = damageOwner;

        HasBeenKilled?.Invoke((int)PredictableFor, damageOwner);
    }

    public void ElympicsUpdate()
    {
        if (!IsDead || !Elympics.IsServer)
            return;

        CurrentDeathTime.Value -= Elympics.TickDuration;

        if (CurrentDeathTime.Value <= 0)
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        if ((GameState)gameStateController.CurrentGameState.Value == GameState.MatchEnded)
            return;

        PlayersSpawner.Instance.SpawnPlayer(playerData);
        PlayerRespawned?.Invoke();
        IsDead.Value = false;
        KillerId.Value = -1;
    }
}
