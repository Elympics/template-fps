using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerBehaviourOnWeaponHit : MonoBehaviour
{
	[SerializeField] private PlayersProvider playersProvider = null;
	[SerializeField] private Animator animator = null;

	private readonly int pointerHitTriggerHash = Animator.StringToHash("PointerHitTrigger");

	private void Awake()
	{
		if (playersProvider.IsReady)
			InitializePointer();
		else
			playersProvider.IsReadyChanged += InitializePointer;
	}

	private void InitializePointer()
	{
		var clientPlayer = playersProvider.ClientPlayer;

		var clientWeapons = clientPlayer.GetComponentsInChildren<Weapon>();
		foreach (Weapon clientWeapon in clientWeapons)
			clientWeapon.WeaponAppliedDamage += OnWeaponAppliedDamage;
	}

	private void OnWeaponAppliedDamage()
	{
		animator.SetTrigger(pointerHitTriggerHash);
	}
}
