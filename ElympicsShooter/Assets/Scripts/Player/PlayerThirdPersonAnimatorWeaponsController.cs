using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThirdPersonAnimatorWeaponsController : MonoBehaviour
{
	[Header("References:")]
	[SerializeField] private Animator thirdPersonAnimator = null;
	[SerializeField] private LoadoutController loadoutController = null;
	[SerializeField] private GameObject[] assignedWeapons = null;

	private readonly int SwapWeaponTrigger = Animator.StringToHash("WeaponInitializeTrigger");

	private GameObject currentActiveAssignedWeapon = null;

	private void Awake()
	{
		currentActiveAssignedWeapon = assignedWeapons[0];

		loadoutController.CurrentEquipedWeaponIndex.ValueChanged += OnWeaponSwap;
	}

	private void OnWeaponSwap(int lastValue, int newValue)
	{
		Debug.Log("On weapon swap " + newValue);
		currentActiveAssignedWeapon.SetActive(false);

		thirdPersonAnimator.SetTrigger(SwapWeaponTrigger);

		currentActiveAssignedWeapon = assignedWeapons[newValue];
		currentActiveAssignedWeapon.SetActive(true);
	}
}
