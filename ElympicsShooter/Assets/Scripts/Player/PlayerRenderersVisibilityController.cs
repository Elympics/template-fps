using UnityEngine;
using Elympics;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerData))]
public class PlayerRenderersVisibilityController : ElympicsMonoBehaviour, IInitializable
{
	[Header("References:")]
	[SerializeField] private GameObject[] firstPersonRendererRoots = null;
	[SerializeField] private GameObject[] thirdPersonRendererRoots = null;

	[Header("Invisible layer:")]
	[SerializeField] private string defaultLayerName = null;
	[SerializeField] private string invisibleLayerName = null;

	public void Initialize()
	{
		var playerData = GetComponent<PlayerData>();

		if (playerData.PlayerId != (int)Elympics.Player)
		{
			ProcessRootsOfRenderersToSetGivenLayer(firstPersonRendererRoots, invisibleLayerName);
			ProcessRootsOfRenderersToSetGivenLayer(thirdPersonRendererRoots, defaultLayerName);
		}
	}

	private void ProcessRootsOfRenderersToSetGivenLayer(GameObject[] rootsOfRenderersToDisable,
		string layerName)
	{
		foreach (GameObject rootOfRenderersToDisable in rootsOfRenderersToDisable)
		{
			var rendererObjectsInChildren =
				Array.ConvertAll(rootOfRenderersToDisable.GetComponentsInChildren<Renderer>(true), x => x.gameObject);
			SetGivenObjectsLayer(rendererObjectsInChildren, layerName);

			rendererObjectsInChildren =
				Array.ConvertAll(rootOfRenderersToDisable.GetComponentsInChildren<Graphic>(true), x => x.gameObject);
			SetGivenObjectsLayer(rendererObjectsInChildren, layerName);
		}
	}

	private void SetGivenObjectsLayer(GameObject[] objectsToChangeLayer,
		string layerName)
	{
		foreach (GameObject objectToChangeLayer in objectsToChangeLayer)
		{
			objectToChangeLayer.layer = LayerMask.NameToLayer(layerName);
		}
	}
}