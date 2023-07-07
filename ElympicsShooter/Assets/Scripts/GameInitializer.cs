using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : ElympicsMonoBehaviour, IUpdatable
{
	[Header("Parameters:")]
	[SerializeField] private float timeToStartMatch = 5.0f;

	public ElympicsFloat CurrentTimeToStartMatch { get; } = new ElympicsFloat(0.0f);
	
	private ElympicsBool gameInitializationEnabled = new ElympicsBool(false);

	private Action OnMatchInitializedAssignedCallback = null;

	public void InitializeMatch(Action OnMatchInitializedCallback)
	{
		OnMatchInitializedAssignedCallback = OnMatchInitializedCallback;

		CurrentTimeToStartMatch.Value = timeToStartMatch;
		gameInitializationEnabled.Value = true;
	}

	public void ElympicsUpdate()
	{
		if (gameInitializationEnabled)
		{
			CurrentTimeToStartMatch.Value -= Elympics.TickDuration;

			if (CurrentTimeToStartMatch < 0.0f)
			{
				OnMatchInitializedAssignedCallback?.Invoke();

				gameInitializationEnabled.Value = false;
			}
		}
	}
}
