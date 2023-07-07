using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchInitializationScreen : MonoBehaviour
{
	[Header("Self References:")]
	[SerializeField] private TextMeshProUGUI countdownToStartMatchText = null;
	[SerializeField] private CanvasGroup screenCanvasGroup = null;
	
	[Header("External References:")]
	[SerializeField] private GameStateController gameStateController = null;
	[SerializeField] private GameInitializer gameInitializer = null;

	private void Awake()
	{
		gameInitializer.CurrentTimeToStartMatch.ValueChanged += UpdateTimeToStartMatchDisplay;

		ProcessScreenViewAtStartOfTheGame();
	}

	private void UpdateTimeToStartMatchDisplay(float lastValue, float newValue)
	{
		countdownToStartMatchText.text = Mathf.Ceil(newValue).ToString();
	}

	private void ProcessScreenViewAtStartOfTheGame()
	{
		SetScreenDisplayBasedOnCurrentGameState(gameStateController.CurrentGameState, gameStateController.CurrentGameState);
		gameStateController.CurrentGameState.ValueChanged += SetScreenDisplayBasedOnCurrentGameState;
	}

	private void SetScreenDisplayBasedOnCurrentGameState(int lastGameState, int newGameState)
	{
		screenCanvasGroup.alpha = (GameState)newGameState == GameState.Prematch ? 1.0f : 0.0f;
	}
}
