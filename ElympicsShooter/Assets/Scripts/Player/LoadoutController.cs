using Elympics;
using UnityEngine;

public class LoadoutController : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
    [Header("References:")]
    [SerializeField] private DeathController deathController = null;

    [SerializeField] private Weapon[] availableWeapons = null;

    [Header("Parameters:")]
    [SerializeField] private float weaponSwapTime = 0.3f;

    public ElympicsInt CurrentEquippedWeaponIndex { get; } = new ElympicsInt(0);

    private ElympicsFloat currentWeaponSwapTime = null;
    private Weapon currentEquippedWeapon = null;
    private int previousWeaponValue = -1;

    public void Initialize()
    {
        currentWeaponSwapTime = new ElympicsFloat(weaponSwapTime);

        DisableAllWeapons();
    }

    private void DisableAllWeapons()
    {
        foreach (Weapon weapon in availableWeapons)
            weapon.SetIsActive(false);
    }

    public void ProcessLoadoutActions(bool weaponPrimaryAction,
        int weaponIndex)
    {
        if (deathController.IsDead.Value)
            return;

        if (weaponIndex != -1 && weaponIndex != CurrentEquippedWeaponIndex.Value)
        {
            SwitchWeapon(weaponIndex);
        }
        else
        {
            if (currentWeaponSwapTime.Value >= weaponSwapTime)
                ProcessWeaponActions(weaponPrimaryAction);
        }
    }

    private void ProcessWeaponActions(bool weaponPrimaryAction)
    {
        if (weaponPrimaryAction)
            ProcessWeaponPrimaryAction();
    }

    private void ProcessWeaponPrimaryAction()
    {
        currentEquippedWeapon.ExecutePrimaryAction();
    }

    public void SwitchWeapon(int weaponIndex)
    {
        CurrentEquippedWeaponIndex.Value = weaponIndex;
    }

    private void UpdateCurrentEquippedWeaponByIndex(int value)
    {
        if (previousWeaponValue == value)
            return;

        previousWeaponValue = value;

        if (currentEquippedWeapon != null)
            currentEquippedWeapon.SetIsActive(false);

        currentEquippedWeapon = availableWeapons[value];
        currentEquippedWeapon.SetIsActive(true);

        currentWeaponSwapTime.Value = 0.0f;
    }

    public void ElympicsUpdate()
    {
        UpdateCurrentEquippedWeaponByIndex(CurrentEquippedWeaponIndex.Value);

        if (currentWeaponSwapTime.Value < weaponSwapTime)
            currentWeaponSwapTime.Value += Elympics.TickDuration;
    }
}