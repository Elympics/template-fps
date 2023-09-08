using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInLoadoutPanelView : MonoBehaviour
{
	[SerializeField] private Sprite disabledWeapon = null;
	[SerializeField] private Sprite enabledWeapon = null;
	[SerializeField] private Image weaponInLoadoutPanelIcon = null;

	[SerializeField] private Sprite ammoSprite = null;
	[SerializeField] private Image ammoImage = null;

	public void UpdateWeaponView(bool isEnabled)
	{
		var spriteToAssign = isEnabled ? enabledWeapon : disabledWeapon;

		weaponInLoadoutPanelIcon.sprite = spriteToAssign;

		if (isEnabled)
			ammoImage.sprite = ammoSprite;
	}
}