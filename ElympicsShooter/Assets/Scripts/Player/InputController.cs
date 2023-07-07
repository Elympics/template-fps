using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elympics;
using System;

[RequireComponent(typeof(InputProvider))]
public class InputController : ElympicsMonoBehaviour, IInputHandler, IInitializable, IUpdatable
{
	[SerializeField] private MovementController movementController = null;
	[SerializeField] private ViewController viewController = null;
	[SerializeField] private LoadoutController loadoutController = null;
	[SerializeField] private HUDController hudController = null;
	[SerializeField] private PlayerData playerData = null;
	[SerializeField] private GameStateController gameController = null;

	private InputProvider inputProvider = null;
	private bool canProcessInputs = true;

	public void Initialize()
	{
		this.inputProvider = GetComponent<InputProvider>();

		UpdateProcessInputsBasedOnCurrentGameState(gameController.CurrentGameState, gameController.CurrentGameState);
		gameController.CurrentGameState.ValueChanged += UpdateProcessInputsBasedOnCurrentGameState;
	}

	private void UpdateProcessInputsBasedOnCurrentGameState(int lastGameState, int newGameState)
	{
		canProcessInputs = (GameState)newGameState == GameState.GameplayMatchRunning;
	}

	public void OnInputForBot(IInputWriter inputSerializer)
	{
		// TODO
	}

	public void OnInputForClient(IInputWriter inputSerializer)
	{
		if (Elympics.Player == ElympicsPlayer.FromIndex(playerData.PlayerId))
			SerializeInput(inputSerializer);
	}

	private void SerializeInput(IInputWriter inputWriter)
	{
		//movement
		inputWriter.Write(inputProvider.Movement.x);
		inputWriter.Write(inputProvider.Movement.y);

		//mouse
		inputWriter.Write(inputProvider.MouseAxis.x);
		inputWriter.Write(inputProvider.MouseAxis.y);
		inputWriter.Write(inputProvider.MouseAxis.z);

		//action buttons
		inputWriter.Write(inputProvider.Jump);
		inputWriter.Write(inputProvider.WeaponPrimaryAction);
		inputWriter.Write(inputProvider.ShowScoreboard);
		inputWriter.Write(inputProvider.WeaponSlot);
	}

	public void ElympicsUpdate()
	{
		float forwardMovement = 0.0f;
		float rightMovement = 0.0f;
		bool jump = false;

		if (canProcessInputs && ElympicsBehaviour.TryGetInput(ElympicsPlayer.FromIndex(playerData.PlayerId), out var inputDeserializer))
		{

			inputDeserializer.Read(out forwardMovement);
			inputDeserializer.Read(out rightMovement);

			inputDeserializer.Read(out float xRotation);
			inputDeserializer.Read(out float yRotation);
			inputDeserializer.Read(out float zRotation);

			inputDeserializer.Read(out jump);
			inputDeserializer.Read(out bool weaponPrimaryAction);
			inputDeserializer.Read(out bool showScoreboard);
			inputDeserializer.Read(out int weaponSlot);

			ProcessMouse(Quaternion.Euler(new Vector3(xRotation, yRotation, zRotation)));

			ProcessLoadoutActions(weaponPrimaryAction, weaponSlot);

			ProcessHUDActions(showScoreboard);
		}

		ProcessMovement(forwardMovement, rightMovement, jump);
	}

	private void ProcessHUDActions(bool showScoreboard)
	{
		hudController.ProcessHUDActions(showScoreboard);
	}

	private void ProcessMouse(Quaternion mouseRotation)
	{
		viewController.ProcessView(mouseRotation);
	}

	private void ProcessLoadoutActions(bool weaponPrimaryAction, int weaponSlot)
	{
		loadoutController.ProcessLoadoutActions(weaponPrimaryAction, weaponSlot);
	}

	private void ProcessMovement(float forwardMovement, float rightMovement, bool jump)
	{
		movementController.ProcessMovement(forwardMovement, rightMovement, jump);
	}
}
