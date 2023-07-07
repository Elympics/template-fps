using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardPlayerCard : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private Image playerIcon = null;
	[SerializeField] private TextMeshProUGUI playerNickname = null;
	[SerializeField] private TextMeshProUGUI playerScoreText = null;

	public int PlayerScore { get; private set; } = -1;

	public void Initialize(PlayerData assignedPlayerData, ElympicsInt playerScore, Action OnPlayerScoreChanged)
	{
		SetupView(assignedPlayerData);

		playerScore.ValueChanged += (int lastValue, int newValue) =>
			{
				UpdateScoreView(newValue);
				OnPlayerScoreChanged();
			};
	}

	private void UpdateScoreView(int newValue)
	{
		PlayerScore = newValue;

		playerScoreText.text = newValue.ToString();
	}

	private void SetupView(PlayerData assignedPlayerData)
	{
		playerIcon.color = assignedPlayerData.ThemeColor;

		playerNickname.text = assignedPlayerData.Nickname;
		playerNickname.color = assignedPlayerData.ThemeColor;
	}
}
