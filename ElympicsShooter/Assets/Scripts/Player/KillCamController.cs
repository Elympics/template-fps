using Elympics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class KillCamController : ElympicsMonoBehaviour, IInitializable
{
	[Header("References:")]
	[SerializeField] private DeathController deathController = null;
	[SerializeField] private PlayersProvider playersProvider = null;
	[SerializeField] private PlayerCamerasController playerCamerasController = null;
	[SerializeField] private PlayerData playerData = null;
	[SerializeField] private CinemachineVirtualCamera thirdPersonCamera = null;

	public void Initialize()
	{
		if (Elympics.IsClient && (int)Elympics.Player == playerData.PlayerId)
		{
			deathController.IsDead.ValueChanged += SetKillCamIsActive;
			deathController.KillerId.ValueChanged += SetupInfoAboutKiller;
		}
	}

	private void SetupInfoAboutKiller(int lastValue, int newValue)
	{
		if (newValue < 0)
			return;

		var killerGameObject = playersProvider.GetPlayerById(newValue);

		thirdPersonCamera.LookAt = killerGameObject.transform;
	}

	private void SetKillCamIsActive(bool lastValue, bool newValue)
	{
		if (newValue)
			playerCamerasController.SetThirdPersonCameraAsActive();
		else
			playerCamerasController.SetDefaultCameraAsActive();
	}
}
