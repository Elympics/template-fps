using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
	[SerializeField] private GameStateController gameStateController = null;

	private void Awake()
	{
		SetCursorLock(true);

		gameStateController.CurrentGameState.ValueChanged += SetCursorLockBasedOnCurrentGameState;
	}

	private void SetCursorLockBasedOnCurrentGameState(int lastValue, int newValue)
	{
		bool cursorLockState = (GameState)newValue != GameState.MatchEnded;

		SetCursorLock(cursorLockState);
	}

	private void SetCursorLock(bool cursorLock)
	{
		if (cursorLock)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}
