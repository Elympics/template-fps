using Cinemachine;
using Elympics;
using UnityEngine;

public class RocketLauncher : Weapon
{
    [SerializeField] private Transform bulletSpawnPoint = null;
    [SerializeField] private ProjectileBullet bulletPrefab = null;
    [SerializeField] private ParticleSystem muzzleFlashEffect = null;
    [SerializeField] private CinemachineVirtualCamera cinemachinePlayerCamera = null;

    public ProjectileBullet BulletPrefab => bulletPrefab;

    protected override void ProcessWeaponAction()
    {
        var bullet = CreateBullet();

        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.transform.rotation;
        bullet.Launch(GetBulletDirection());

        muzzleFlashEffect.Play();

        WeaponShot?.Invoke();
    }

    private Vector3 GetBulletDirection()
    {
        // We use raycast only to get direction for the bullet towards center of the screen - the crosshair (needed because weapon is not in the center of camera)
        if (Physics.Raycast(cinemachinePlayerCamera.transform.position, cinemachinePlayerCamera.transform.forward,
                out RaycastHit hit, Mathf.Infinity))
            return (hit.point - bulletSpawnPoint.transform.position).normalized;

        return bulletSpawnPoint.transform.forward;
    }

    private ProjectileBullet CreateBullet()
    {
        var bullet = ElympicsInstantiate(bulletPrefab.gameObject.name, this.PredictableFor);

        var projectileBullet = bullet.GetComponent<ProjectileBullet>();

        projectileBullet.SetOwner(Owner.gameObject.transform.root.gameObject.GetComponent<ElympicsBehaviour>());
        projectileBullet.SetApplyingDamageCallback(WeaponAppliedDamage);

        return projectileBullet;
    }
}