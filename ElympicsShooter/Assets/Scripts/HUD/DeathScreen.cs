using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
	[SerializeField] private PlayersProvider playersProvider = null;
	[SerializeField] private CanvasGroup canvasGroup = null;
	[SerializeField] private TextMeshProUGUI deathTimerText = null;
	[SerializeField] private GameStateController gameStateController = null;

	private bool displayingDeathScreenDisabled = false;

	private void Start()
	{
		if (playersProvider.IsReady)
			SubscribeToDeathController();
		else
			playersProvider.IsReadyChanged += SubscribeToDeathController;

		gameStateController.CurrentGameState.ValueChanged += ProcessLockDisplayingDeathScreenBasedOnCurrentGameState;
	}

	private void ProcessLockDisplayingDeathScreenBasedOnCurrentGameState(int lastValue, int newValue)
	{
		if ((GameState)newValue == GameState.MatchEnded)
		{
			displayingDeathScreenDisabled = true;

			canvasGroup.alpha = 0.0f;
		}
	}

	private void SubscribeToDeathController()
	{
		var clientPlayerData = playersProvider.ClientPlayer;
		clientPlayerData.DeathController.CurrentDeathTime.ValueChanged += UpdateDeathTimerView;
		clientPlayerData.DeathController.IsDead.ValueChanged += UpdateDeathScreenView;
	}

	private void UpdateDeathScreenView(bool lastValue, bool newValue)
	{
		if (displayingDeathScreenDisabled)
			return;

		canvasGroup.alpha = newValue ? 1.0f : 0.0f;
	}

	private void UpdateDeathTimerView(float lastValue, float newValue)
	{
		deathTimerText.text = Mathf.Ceil(newValue).ToString();
	}
}
