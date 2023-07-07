using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;

public class PlayerScoresManager : ElympicsMonoBehaviour, IInitializable
{
	[SerializeField] private PlayersProvider playersProvider = null;
	[SerializeField] private int pointsRequiredToWin = 10;

	private ElympicsArray<ElympicsInt> playerScores = null;
	public ElympicsInt WinnerPlayerId { get; } = new ElympicsInt(-1);

	public bool IsReady { get; private set; } = false;
	public event Action IsReadyChanged = null;

	public void Initialize()
	{
		if (playersProvider.IsReady)
			SetupManager();
		else
			playersProvider.IsReadyChanged += SetupManager;
	}

	private void SetupManager()
	{
		PreparePlayerScores();
		SubscribeToDeathControllers();

		IsReady = true;
		IsReadyChanged?.Invoke();
	}

	private void SubscribeToDeathControllers()
	{
		foreach (PlayerData playerData in playersProvider.AllPlayersInScene)
		{
			if (playerData.TryGetComponent(out DeathController deathController))
			{
				deathController.HasBeenKilled += ProcessPlayerDeath;
			}
		}
	}

	private void ProcessPlayerDeath(int victim, int killer)
	{
		//If player killed himself subtract one point
		if (victim == killer)
			playerScores.Values[killer].Value--;
		//otherwise add point
		else
			playerScores.Values[killer].Value++;

		//Check if points required to win reached
		if (Elympics.IsServer && playerScores.Values[killer].Value >= pointsRequiredToWin)
		{
			WinnerPlayerId.Value = killer;
		}
	}

	private void PreparePlayerScores()
	{
		var numberOfPlayers = playersProvider.AllPlayersInScene.Length;

		ElympicsInt[] localPlayerScoresArray = new ElympicsInt[numberOfPlayers];

		for (int i = 0; i < numberOfPlayers; i++)
		{
			localPlayerScoresArray[i] = new ElympicsInt(0);
		}

		playerScores = new ElympicsArray<ElympicsInt>(localPlayerScoresArray);
	}

	public ElympicsInt GetScoreForPlayer(int playerId)
	{
		return playerScores.Values[playerId];
	}

}
