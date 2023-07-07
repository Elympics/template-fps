using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutPanel : MonoBehaviour
{
	[SerializeField] private WeaponInLoadoutPanelView[] loadoutWeaponIcons = null;
	[SerializeField] private PlayersProvider playersProvider = null;

	private int currentEquipedWeaponIcon = 0;

	private void Awake()
	{
		if (playersProvider.IsReady)
			InitializePanelBasedOnClientPlayer();
		else
			playersProvider.IsReadyChanged += InitializePanelBasedOnClientPlayer;

		foreach (WeaponInLoadoutPanelView loadoutWeaponIcon in loadoutWeaponIcons)
			loadoutWeaponIcon.UpdateWeaponView(false);

		currentEquipedWeaponIcon = 0;
		UpdateCurrentEquipedWeaponView(0, 0);
	}

	private void InitializePanelBasedOnClientPlayer()
	{
		var clientPlayer = playersProvider.ClientPlayer;

		clientPlayer.LoadoutController.CurrentEquipedWeaponIndex.ValueChanged += UpdateCurrentEquipedWeaponView;
	}

	private void UpdateCurrentEquipedWeaponView(int lastValue, int newValue)
	{
		loadoutWeaponIcons[currentEquipedWeaponIcon].UpdateWeaponView(false);

		currentEquipedWeaponIcon = newValue;

		loadoutWeaponIcons[currentEquipedWeaponIcon].UpdateWeaponView(true);
	}
}
