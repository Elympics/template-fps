using Cinemachine;
using Elympics;
using UnityEngine;

public class KillCamController : ElympicsMonoBehaviour
{
    [Header("References:")]
    [SerializeField] private DeathController deathController = null;
    [SerializeField] private PlayersProvider playersProvider = null;
    [SerializeField] private PlayerCamerasController playerCamerasController = null;
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private CinemachineVirtualCamera thirdPersonCamera = null;

    private void Awake()
    {
        deathController.HasBeenKilled += (victimId,
            killerId) => SetupKillCamProperties();
    }

    private void SetupInfoAboutKiller(int value)
    {
        if (value < 0)
            return;

        var killerGameObject = playersProvider.GetPlayerById(value);

        thirdPersonCamera.LookAt = killerGameObject.transform;
    }

    private void SetKillCamIsActive(bool value)
    {
        if (value)
            playerCamerasController.SetThirdPersonCameraAsActive();
        else
            playerCamerasController.SetDefaultCameraAsActive();
    }


    [ElympicsRpc(ElympicsRpcDirection.ServerToPlayers)]
    private void SetupKillCamProperties()
    {
        if ((int)Elympics.Player != playerData.PlayerId)
            return;

        SetKillCamIsActive(deathController.IsDead.Value);
        SetupInfoAboutKiller(deathController.KillerId.Value);
    }
}