using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField] private PlayersProvider playersProvider = null;
	[SerializeField] private Slider healthSlider = null;
	[SerializeField] private TextMeshProUGUI healthCurrentValue = null;

	private void Start()
	{
		if (playersProvider.IsReady)
			SubscribeToStatsController();
		else
			playersProvider.IsReadyChanged += SubscribeToStatsController;
	}

	private void SubscribeToStatsController()
	{
		var clientPlayerData = playersProvider.ClientPlayer;
		clientPlayerData.StatsController.HealthValueChanged += UpdateHealthView;
	}

	private void UpdateHealthView(float currentHealth, float maxHealth)
	{
		healthCurrentValue.text = Mathf.Round(currentHealth).ToString();

		healthSlider.value = currentHealth / maxHealth;
	}
}
