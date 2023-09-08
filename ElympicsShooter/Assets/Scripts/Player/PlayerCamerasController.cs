using Elympics;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(PlayerData))]
public class PlayerCamerasController : ElympicsMonoBehaviour, IInitializable
{
	[System.Serializable]
	public struct CMVirtualCameraWithASsignedLayerMask
	{
		public CinemachineVirtualCamera VirtualCamera;
		public LayerMask AssignedLayerMask;
	}

	[Header("References:")]
	[SerializeField] private Camera brainCamera = null;
	[SerializeField] private Camera nonClippingCamera = null;

	[Header("Parameters:")]
	[SerializeField] private CMVirtualCameraWithASsignedLayerMask firstPersonCamera;
	[SerializeField] private CMVirtualCameraWithASsignedLayerMask thirdPersonCamera;

	private CinemachineVirtualCamera[] allCamerasInPlayer = null;
	private UniversalAdditionalCameraData brainCameraData = null;

	public void Initialize()
	{
		var playerData = GetComponent<PlayerData>();
		allCamerasInPlayer = GetComponentsInChildren<CinemachineVirtualCamera>();
		brainCameraData = brainCamera.GetComponent<UniversalAdditionalCameraData>();

		DisableAllCamerasInPlayer();

		InitializeCamerasAtGameStart(playerData);
	}

	private void InitializeCamerasAtGameStart(PlayerData playerData)
	{
		if (Elympics.IsClient && (int)Elympics.Player == playerData.PlayerId)
			firstPersonCamera.VirtualCamera.Priority = (int)VirtualCamPriority.Active;
	}

	public void SetDefaultCameraAsActive()
	{
		SetUpCamera(true);
	}

	public void SetThirdPersonCameraAsActive()
	{
		SetUpCamera(false);
	}

	private void SetUpCamera(bool firstPerson)
	{
		DisableAllCamerasInPlayer();
		SetCameraAsActive(firstPerson ? firstPersonCamera : thirdPersonCamera);

		brainCameraData.renderPostProcessing =
			!firstPerson; // we want only one camera to have enabled rendering at time
		nonClippingCamera.enabled =
			firstPerson; // we want it disabled when in TPP to make first person hands not rendered
	}

	private void SetCameraAsActive(CMVirtualCameraWithASsignedLayerMask camera)
	{
		camera.VirtualCamera.Priority = (int)VirtualCamPriority.Active;

		brainCamera.cullingMask = camera.AssignedLayerMask;
	}

	private void DisableAllCamerasInPlayer()
	{
		foreach (CinemachineVirtualCamera camera in allCamerasInPlayer)
			camera.Priority = (int)VirtualCamPriority.Disabled;
	}
}