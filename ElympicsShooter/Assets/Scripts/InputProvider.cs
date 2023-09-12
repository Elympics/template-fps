using UnityEngine;

public class InputProvider : MonoBehaviour
{
    [SerializeField] private float mouseSensivity = 1.5f;
    [SerializeField, Tooltip("X: max downward tilt, Y: max upward tilt")] private Vector2 verticalAngleLimits = Vector2.zero;
    [SerializeField] private bool invertedMouseXAxis = false;
    [SerializeField] private bool invertedMouseYAxis = false;

    private Vector2 movement = Vector2.zero;
    public Vector2 Movement => movement;

    private Vector3 mouseAxis = Vector3.zero;
    public Vector3 MouseAxis => mouseAxis;

    public bool Jump { get; private set; }
    public bool WeaponPrimaryAction { get; private set; }
    public bool ShowScoreboard { get; private set; }
    public int WeaponSlot { get; private set; }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        var mouseX = Input.GetAxis("Mouse X") * (invertedMouseXAxis ? -1 : 1);
        var mouseY = Input.GetAxis("Mouse Y") * (invertedMouseYAxis ? -1 : 1);
        var newMouseAngles = mouseAxis + new Vector3(mouseY, mouseX) * mouseSensivity;
        mouseAxis = FixTooLargeMouseAngles(newMouseAngles);

        Jump = Input.GetButton("Jump");
        WeaponPrimaryAction = Input.GetButton("Fire1");
        ShowScoreboard = Input.GetKey(KeyCode.Tab);

        WeaponSlot = Input.GetKey(KeyCode.Alpha1) ? 0 :
            Input.GetKey(KeyCode.Alpha2) ? 1 : -1;
    }

    private Vector3 FixTooLargeMouseAngles(Vector3 mouseAngles)
    {
        mouseAngles.x = Mathf.Clamp(mouseAngles.x, verticalAngleLimits.x, verticalAngleLimits.y);

        return mouseAngles;
    }
}