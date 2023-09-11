using Elympics;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBullet : ElympicsMonoBehaviour, IUpdatable
{
    [Header("Parameters:")]
    [SerializeField] protected float speed = 5.0f;
    [SerializeField] protected float lifeTime = 5.0f;
    [SerializeField] protected float timeToDestroyOnExplosion = 1.0f;

    [Header("References:")]
    [SerializeField] private ExplosionArea explosionArea = null;
    [SerializeField] private GameObject bulletMeshRoot = null;
    [SerializeField] protected new Rigidbody rigidbody = null;
    [SerializeField] protected new Collider collider = null;

    public float LifeTime => lifeTime;

    protected ElympicsBool bulletExploded = new ElympicsBool(false);

    private ElympicsGameObject owner = new ElympicsGameObject();
    private ElympicsFloat deathTimer = new ElympicsFloat(0.0f);
    private Vector3 direction;

    public void SetApplyingDamageCallback(Action weaponAppliedDamage)
    {
        explosionArea.SetApplyingDamageCallback(weaponAppliedDamage);
    }

    public void SetOwner(ElympicsBehaviour owner)
    {
        this.owner.Value = owner;
    }

    public void Launch(Vector3 direction)
    {
        ChangeMovementState(true);
        this.direction = direction;

        ChangeBulletVelocity(direction);
    }

    private void ChangeBulletVelocity(Vector3 direction)
    {
        rigidbody.velocity = direction * speed;
    }

    private void DestroyProjectile()
    {
        ElympicsDestroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (owner.Value == null)
            return;

        if (collision.transform.root.gameObject == owner.Value.gameObject)
            return;

        ChangeMovementState(false);

        TryDetonateProjectile(true);
    }

    private void ChangeMovementState(bool active)
    {
        rigidbody.isKinematic = !active;
        //rigidbody.useGravity = active; // can be uncommented if you want gravity-dependent projectile
        collider.enabled = active;
    }

    private void TryDetonateProjectile(bool newValue)
    {
        if (newValue)
            LaunchExplosion();
    }

    public void ElympicsUpdate()
    {
        deathTimer.Value += Elympics.TickDuration;

        if ((!bulletExploded.Value && deathTimer.Value >= lifeTime)
            || (bulletExploded.Value && deathTimer.Value >= timeToDestroyOnExplosion))
        {
            DestroyProjectile();
        }
    }

    private void LaunchExplosion()
    {
        bulletMeshRoot.SetActive(false);

        explosionArea.Detonate();

        bulletExploded.Value = true;
        deathTimer.Value = 0.0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(this.transform.position, direction);
    }
}