using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : ElympicsMonoBehaviour, IInitializable
{
	[Header("Parameters:")]
	[SerializeField] private float maxHealth = 100.0f;

	[Header("References:")]
	[SerializeField] private DeathController deathController = null;

	private ElympicsFloat health = new ElympicsFloat(0);
	public event Action<float, float> HealthValueChanged = null;

	public void Initialize()
	{
		health.Value = maxHealth;
		health.ValueChanged += OnHealthValueChanged;

		deathController.PlayerRespawned += ResetPlayerStats;
	}

	private void ResetPlayerStats()
	{
		health.Value = maxHealth;
	}

	public void ChangeHealth(float value, int damageOwner)
	{
		if (!Elympics.IsServer || deathController.IsDead)
			return;

		health.Value += value;

		if (health.Value <= 0.0f)
			deathController.ProcessPlayersDeath(damageOwner);
	}

	private void OnHealthValueChanged(float lastValue, float newValue)
	{
		HealthValueChanged?.Invoke(newValue, maxHealth);
	}
}
