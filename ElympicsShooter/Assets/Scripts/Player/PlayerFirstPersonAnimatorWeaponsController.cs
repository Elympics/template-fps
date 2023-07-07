using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirstPersonAnimatorWeaponsController : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private Animator handsRootAnimator = null;
	[SerializeField] private Animator playerHandsAnimator = null;
	[SerializeField] private LoadoutController loadoutController = null;

	//Hands Root Animator Parameters
	private readonly int SwapWeaponTrigger = Animator.StringToHash("WeaponInitializeTrigger");

	//Player Hands Animator Parameters
	private readonly int ActiveWeaponIndex = Animator.StringToHash("ActiveWeaponIndex");
	private readonly int WeaponShotTrigger = Animator.StringToHash("ShotTrigger");

	private void Awake()
	{
		loadoutController.CurrentEquipedWeaponIndex.ValueChanged += OnWeaponSwap;

		var weaponsAssignedToCharacter = this.transform.root.GetComponentsInChildren<Weapon>(true);
		foreach (Weapon weaponAssignedToCharacter in weaponsAssignedToCharacter)
			weaponAssignedToCharacter.WeaponShot += OnWeaponShot;
	}

	private void OnWeaponShot()
	{
		handsRootAnimator.SetTrigger(WeaponShotTrigger);
	}

	private void OnWeaponSwap(int lastValue, int newValue)
	{
		handsRootAnimator.SetTrigger(SwapWeaponTrigger);
		playerHandsAnimator.SetInteger(ActiveWeaponIndex, newValue);
	}
}
