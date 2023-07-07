using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerData))]
public class PlayerThemeApplier : MonoBehaviour
{
	[SerializeField] private SkinnedMeshRenderer[] themeBasedRenderers = null;
	[SerializeField] private SkinnedMeshRenderer[] themeBasedMetallicRenderers = null;

	private void Awake()
	{
		var playerData = GetComponent<PlayerData>();

		ApplyTheme(playerData.ThemeMaterial, themeBasedRenderers);
		ApplyTheme(playerData.ThemeMaterialMetallic, themeBasedMetallicRenderers);
	}

	private void ApplyTheme(Material themeMaterial, SkinnedMeshRenderer[] renderers)
	{
		foreach (SkinnedMeshRenderer themeBasedRenderer in renderers)
			themeBasedRenderer.material = themeMaterial;
	}
}
