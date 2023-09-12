using Cinemachine;
using Elympics;
using System;
using UnityEngine;

public class RailGun : Weapon
{
    [Header("Parameters:")]
    [SerializeField] private float loadingTime = 1.0f;

    [Header("References:")]
    [SerializeField] private CinemachineVirtualCamera cinemachinePlayerCamera = null;

    private ElympicsFloat currentLoadingTime = new ElympicsFloat(0.0f);
    private ElympicsBool isLoadingToShot = new ElympicsBool(false);
    private double previousLoadingTime;

    public event Action<float, float> LoadingTimeChanged;
    public event Action<RaycastHit> WeaponFired;

    protected override void ProcessWeaponAction()
    {
        if (isLoadingToShot.Value)
            return;

        currentLoadingTime.Value = 0.0f;
        isLoadingToShot.Value = true;
    }

    public override void ElympicsUpdate()
    {
        base.ElympicsUpdate();

        if (isLoadingToShot.Value)
        {
            if (currentLoadingTime.Value >= loadingTime)
                ChangeCurrentLoadingTime(0.0f);
            else
                ChangeCurrentLoadingTime(currentLoadingTime.Value + Elympics.TickDuration);
        }

        HandleCurrentLoadingTimeChanged(currentLoadingTime.Value);
    }

    private void HandleCurrentLoadingTimeChanged(float newValue)
    {
        if (previousLoadingTime == newValue)
            return;

        // Preventing of multiple rays in case of renconciliation
        if (previousLoadingTime >= loadingTime && newValue < loadingTime)
            ProcessRayShot();

        previousLoadingTime = newValue;
    }

    private void ProcessRayShot()
    {
        RaycastHit hit;

        if (Physics.Raycast(cinemachinePlayerCamera.transform.position, cinemachinePlayerCamera.transform.forward,
                out hit, Mathf.Infinity))
        {
            if (hit.transform.TryGetComponent<StatsController>(out StatsController statsController))
            {
                statsController.ChangeHealth(-damage, (int)PredictableFor);

                WeaponAppliedDamage?.Invoke();
            }
        }

        WeaponFired?.Invoke(hit);
        WeaponShot?.Invoke();

        isLoadingToShot.Value = false;
        LoadingTimeChanged?.Invoke(0, loadingTime);
    }

    public override void SetIsActive(bool isActive)
    {
        base.SetIsActive(isActive);

        if (!isActive)
            isLoadingToShot.Value = false;

        ChangeCurrentLoadingTime(0.0f);
    }

    private void ChangeCurrentLoadingTime(float newCurrentLoadingTime)
    {
        currentLoadingTime.Value = newCurrentLoadingTime;
        LoadingTimeChanged?.Invoke(currentLoadingTime.Value, loadingTime);
    }
}