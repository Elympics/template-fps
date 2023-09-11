using UnityEngine;

public class GroundContact
{
    public float SlopeAngle { get; private set; }
    public Vector3 Norm { get; private set; }

    public GroundContact(float slopeAngle,
        Vector3 norm)
    {
        SlopeAngle = slopeAngle;
        Norm = norm;
    }
}