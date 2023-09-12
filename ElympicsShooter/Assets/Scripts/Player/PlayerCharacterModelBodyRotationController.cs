using Elympics;
using UnityEngine;

public class PlayerCharacterModelBodyRotationController : ElympicsMonoBehaviour
{
    [Header("References:")]
    [SerializeField] private Transform rotatingBone = null;
    [SerializeField] private Transform lookAtTarget = null;
    [SerializeField] private DeathController deathController = null;

    public void LateUpdate()
    {
        if (!deathController.IsDead.Value)
            rotatingBone.transform.LookAt(lookAtTarget);
    }
}