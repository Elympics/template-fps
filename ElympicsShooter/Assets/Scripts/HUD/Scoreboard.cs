using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private ScoreboardPlayerCard scoreboardPlayerCardPrefab = null;
	[SerializeField] private Transform cardsContainer = null;
	[SerializeField] private CanvasGroup canvasGroup = null;
	[SerializeField] private PlayersProvider playersProvider = null;
	[SerializeField] private PlayerScoresManager playerScoresManager = null;
	[SerializeField] private GameOverScreenViewController gameOverScreenViewController = null;

	private HUDController clientHudController = null;
	private List<ScoreboardPlayerCard> createdScoreboardPlayerCards = new List<ScoreboardPlayerCard>();

	private void Awake()
	{
		if (playersProvider.IsReady)
			SetupScoreboard();
		else
			playersProvider.IsReadyChanged += SetupScoreboard;
	}

	private void SetupScoreboard()
	{
		if (playersProvider.ClientPlayer.TryGetComponent(out HUDController hudController))
		{
			clientHudController = hudController;
			hudController.ShowScoreboardValueChanged += SetScoreboardDisplayStatus;
		}

		if (playerScoresManager.IsReady)
			SubscribeToPlayerScoreManager();
		else
			playerScoresManager.IsReadyChanged += SubscribeToPlayerScoreManager;
	}

	private void SetScoreboardDisplayStatus(bool showScoreboard)
	{
		canvasGroup.alpha = showScoreboard ? 1.0f: 0.0f;
	}

	private void SubscribeToPlayerScoreManager()
	{
		CreatePlayerCards();

		playerScoresManager.WinnerPlayerId.ValueChanged += OnWinnerPlayerIdSet;
	}

	private void OnWinnerPlayerIdSet(int lastValue, int newValue)
	{
		if (newValue < 0)
			return;

		clientHudController.ShowScoreboardValueChanged -= SetScoreboardDisplayStatus;

		SetScoreboardDisplayStatus(true);

		gameOverScreenViewController.ShowGameOverScreen(playersProvider.AllPlayersInScene[newValue]);
	}

	private void CreatePlayerCards()
	{
		foreach (PlayerData playerData in playersProvider.AllPlayersInScene)
		{
			var createdCard = Instantiate(scoreboardPlayerCardPrefab, cardsContainer);
			createdCard.Initialize(playerData, playerScoresManager.GetScoreForPlayer(playerData.PlayerId), RefreshOrderOfScoreboardPlayerCard);

			createdScoreboardPlayerCards.Add(createdCard);
		}
	}

	private void RefreshOrderOfScoreboardPlayerCard()
	{
		var sortedCards = createdScoreboardPlayerCards.OrderByDescending(x => x.PlayerScore);

		int siblingIndex = 0;

		foreach (ScoreboardPlayerCard playerCard in sortedCards)
		{
			playerCard.transform.SetSiblingIndex(siblingIndex);
			siblingIndex++;
		}
	}
}
