using UnityEngine;

public class LaserVisuals : MonoBehaviour
{
    [SerializeField] private LineRenderer trailRenderer = null;
    [SerializeField] private ParticleSystem[] startingPointEffects = null;
    [SerializeField] private ParticleSystem[] endingPointEffects = null;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetPoints(Vector3 start,
        Vector3 end)
    {
        trailRenderer.SetPosition(0, start);
        trailRenderer.SetPosition(1, end);

        Vector3 direction = end - start;

        foreach (var effect in startingPointEffects)
        {
            effect.transform.position = start;
            effect.transform.forward = direction;
        }

        foreach (var effect in endingPointEffects)
        {
            effect.transform.position = end;
            effect.transform.forward = direction;
        }
    }
}