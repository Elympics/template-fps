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
    [SerializeField] private Transform playerRig = null;
    [SerializeField] private Animator animator = null;

    private Collider[] rigColliders;
    private Rigidbody[] rigRigidbodies;

    public ElympicsBool IsDead { get; } = new ElympicsBool(false);
    public ElympicsFloat CurrentDeathTime { get; } = new ElympicsFloat(0.0f);
    public ElympicsInt KillerId { get; } = new ElympicsInt(-1);

    public event Action PlayerRespawned = null;
    public event Action<int, int> HasBeenKilled = null;


    private PlayerData playerData = null;
    private bool physicsChanged;

    public void Initialize()
    {
        playerData = GetComponent<PlayerData>();

        // ragdoll setup
        rigColliders = playerRig.GetComponentsInChildren<Collider>();
        rigRigidbodies = playerRig.GetComponentsInChildren<Rigidbody>();
        SetRagdollState(false);
    }

    private void HandleAfterlifePhysics(bool value)
    {
        if (physicsChanged == value)
            return;

        physicsChanged = value;
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.useGravity = !value;

        foreach (var collider in playerColliders)
        {
            collider.enabled = !value;
        }

        SetRagdollState(value);
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
        HandleAfterlifePhysics(IsDead.Value);

        if (!IsDead.Value)
            return;

        ServerPlayerRespawn();
    }

    private void ServerPlayerRespawn()
    {
        if (!Elympics.IsServer)
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

    private void SetRagdollState(bool state)
    {
        foreach (var collider in rigColliders)
        {
            collider.enabled = state;
        }

        foreach (var rb in rigRigidbodies)
        {
            rb.isKinematic = !state;
        }

        animator.enabled = !state;
    }
}