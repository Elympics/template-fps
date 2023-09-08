using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Elympics;
using Cinemachine;

[RequireComponent(typeof(RailGun))]
public class RailGunVisuals : ElympicsMonoBehaviour, IInitializable
{
	[Header("References:")]
	[SerializeField] private Image loadingBar = null;
	[SerializeField] private Canvas loadingBarCanvasRoot = null;
	[SerializeField] private Transform bulletSpawnPoint = null;
	[SerializeField] private LaserVisuals laserVisualsPrefab = null;
	[SerializeField] private CinemachineVirtualCamera cinemachinePlayerCamera = null;

	[Header("Parameters:")]
	[SerializeField] private float trailLifetime = 0.5f;

	private ElympicsArray<ElympicsVector3> railRenderPoints =
		new ElympicsArray<ElympicsVector3>(new ElympicsVector3[] { new ElympicsVector3(), new ElympicsVector3() });

	private Coroutine trailDeathTimerCoroutine = null;
	private LaserVisuals laserVisuals = null;

	public void Initialize()
	{
		laserVisuals = Instantiate(laserVisualsPrefab, Vector3.zero, Quaternion.identity);

		var railGun = GetComponent<RailGun>();

		railGun.LoadingTimeChanged += ProcessLoadingTimeChanged;
		railGun.WeaponFired += ProcessWeaponFired;

		railRenderPoints.Values[0].ValueChanged += UpdateLaserPoints;
		railRenderPoints.Values[1].ValueChanged += UpdateLaserPoints;

		DisableLoadingBarsIfItsNotElympicsClient();
	}

	private void DisableLoadingBarsIfItsNotElympicsClient()
	{
		if (PredictableFor != Elympics.Player)
		{
			loadingBarCanvasRoot.gameObject.SetActive(false);
		}
	}

	private void UpdateLaserPoints(Vector3 lastValue,
		Vector3 newValue)
	{
		laserVisuals.SetPoints(railRenderPoints.Values[0].Value, railRenderPoints.Values[1].Value);

		CastRay();
	}

	private void ProcessWeaponFired(RaycastHit hit)
	{
		railRenderPoints.Values[0].Value = bulletSpawnPoint.transform.position;

		if (hit.collider != null)
		{
			railRenderPoints.Values[1].Value = hit.point;
		}
		else
		{
			railRenderPoints.Values[1].Value = cinemachinePlayerCamera.transform.position +
			                                   (cinemachinePlayerCamera.transform.forward * 100.0f);
		}

		CastRay();
	}

	private void CastRay()
	{
		if (trailDeathTimerCoroutine != null)
			StopCoroutine(trailDeathTimerCoroutine);

		laserVisuals.gameObject.SetActive(true);

		trailDeathTimerCoroutine = StartCoroutine(TrailDeathTimer());
	}

	private void ProcessLoadingTimeChanged(float currentLoadingValue,
		float maxLoadingValue)
	{
		var fillAmountvalue = currentLoadingValue / maxLoadingValue;

		loadingBar.fillAmount = fillAmountvalue;
	}

	private IEnumerator TrailDeathTimer()
	{
		yield return new WaitForSeconds(trailLifetime);

		laserVisuals.gameObject.SetActive(false);
	}
}