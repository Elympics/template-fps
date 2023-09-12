using Elympics;
using System;
using UnityEngine;

public class ExplosionArea : ElympicsMonoBehaviour
{
    [Header("Parameters:")]
    [SerializeField] private float explosionDamage = 10.0f;
    [SerializeField] private float explosionRange = 2.0f;

    [Header("References:")]
    [SerializeField] private ParticleSystem explosionPS = null;
    [SerializeField] private ElympicsMonoBehaviour bulletOwner = null;

    private Action weaponAppliedDamageCallback = null;

    public void Detonate()
    {
        DetectTargetsInExplosionRange();

        explosionPS.Play();
    }

    public void SetApplyingDamageCallback(Action weaponAppliedDamage)
    {
        weaponAppliedDamageCallback = weaponAppliedDamage;
    }

    private void DetectTargetsInExplosionRange()
    {
        Collider[] objectsInExplosionRange = Physics.OverlapSphere(this.transform.position, explosionRange);
        bool damageApplied = false;

        foreach (Collider objectInExplosionRange in objectsInExplosionRange)
        {
            if (TargetIsNotBehindObstacle(objectInExplosionRange.gameObject))
                damageApplied |= TryToApplyDamageToTarget(objectInExplosionRange.gameObject);
        }

        if (damageApplied)
            weaponAppliedDamageCallback?.Invoke();
    }

    private bool TryToApplyDamageToTarget(GameObject objectInExplosionRange)
    {
        if (objectInExplosionRange.TryGetComponent<StatsController>(out StatsController targetStatsController))
        {
            //TODO: Add damage modification depending on distance from explosion center
            targetStatsController.ChangeHealth(-explosionDamage, (int)bulletOwner.PredictableFor);
            return true;
        }

        return false;
    }

    private bool TargetIsNotBehindObstacle(GameObject objectInExplosionRange)
    {
        var directionToObjectInExplosionRange =
            (objectInExplosionRange.transform.position - this.transform.position).normalized;

        if (Physics.Raycast(this.transform.position, directionToObjectInExplosionRange, out RaycastHit hit,
                explosionRange))
        {
            Debug.Log("I hit " + hit.transform.gameObject.name);
            return hit.transform.gameObject == objectInExplosionRange;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, explosionRange);
    }
}