using Elympics;
using System;
using UnityEngine;

public abstract class Weapon : ElympicsMonoBehaviour, IInitializable, IUpdatable
{
	[SerializeField] protected float damage = 0.0f;
	[SerializeField] [Tooltip("Shots per minute")] protected float fireRate = 60.0f;
	[SerializeField] private GameObject meshContainer = null;

	public Action WeaponShot = null;
	public Action WeaponAppliedDamage = null;

	protected ElympicsFloat currentTimeBetweenShoots = new ElympicsFloat();

	protected float timeBetweenShoots = 0.0f;
	public float TimeBetweenShoots => timeBetweenShoots;

	protected bool IsReady => currentTimeBetweenShoots >= timeBetweenShoots;

	public GameObject Owner => this.transform.root.gameObject;

	public virtual void Initialize()
	{
		CalculateTimeBetweenShoots();
	}

	public void CalculateTimeBetweenShoots()
	{
		if (fireRate > 0)
			timeBetweenShoots = 60.0f / fireRate;
		else
			timeBetweenShoots = 0.0f;
	}

	public void ExecutePrimaryAction()
	{
		ExecuteWeaponActionIfReady();
	}

	private void ExecuteWeaponActionIfReady()
	{
		if (IsReady)
		{
			ProcessWeaponAction();

			currentTimeBetweenShoots.Value = 0.0f;
		}
	}

	protected abstract void ProcessWeaponAction();

	public virtual void SetIsActive(bool isActive)
	{
		meshContainer.SetActive(isActive);
	}

	public virtual void ElympicsUpdate()
	{
		if (!IsReady)
		{
			currentTimeBetweenShoots.Value += Elympics.TickDuration;
		}
	}
}
