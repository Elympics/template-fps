using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;

public class GameStateController : ElympicsMonoBehaviour, IInitializable
{
	[Header("References:")]
	[SerializeField] private GameInitializer gameInitializer = null;
	[SerializeField] private PlayerScoresManager playerScoresManager = null;

	public ElympicsInt CurrentGameState { get; } = new ElympicsInt((int)GameState.Prematch);

	public void Initialize()
	{
		gameInitializer.InitializeMatch(() => ChangeGameState(GameState.GameplayMatchRunning));

		playerScoresManager.WinnerPlayerId.ValueChanged += ProcessGameStateBasedOnWinnerIdChanged;
	}

	private void ProcessGameStateBasedOnWinnerIdChanged(int lastValue, int newValue)
	{
		if (newValue >= 0)	
		{
			ChangeGameState(GameState.MatchEnded);
		}
	}

	private void ChangeGameState(GameState newGameState)
	{
		CurrentGameState.Value = (int)newGameState;
	}
}
