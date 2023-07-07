using System;
using System.Collections;
using UnityEngine;

public class HUDController : MonoBehaviour
{
	public event Action<bool> ShowScoreboardValueChanged;

	private bool showScoreboardValue = false;

	public void ProcessHUDActions(bool showScoreboard)
	{
		if (showScoreboardValue != showScoreboard)
		{
			showScoreboardValue = showScoreboard;
			ShowScoreboardValueChanged?.Invoke(showScoreboardValue);
		}
	}
}