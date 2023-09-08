using Elympics;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : ElympicsMonoBehaviour, IUpdatable
{
	private const float MaxClimbAngle = Mathf.PI / 3.0f;
	private const float GroundCheckSphereScale = 0.95f;
	private readonly Vector3 isGroundedCheckOffset = 0.05f * Vector3.down;

	[Header("References:")]
	[SerializeField] private DeathController deathController = null;
	[SerializeField] private GameStateController gameStateController = null;
	[SerializeField] private CapsuleCollider physicsCollider = null;
	[SerializeField] private LayerMask groundLayer;

	[Header("Parameters:")]
	[SerializeField] private float movementSpeed = 0.0f;
	[SerializeField] private float acceleration = 0.0f;
	[SerializeField] private float jumpForce = 0.0f;
	
	public event Action<Vector3> MovementValuesChanged;
	public event Action PlayerJumped;
	public event Action<bool> IsGroundedStateUpdate;

	private new Rigidbody rigidbody = null;
	private bool jumpedInPreviousFrame = false;
	private Vector3 checkSphereVerticalOffset;
	private float checkSphereRadius;
	private int previousGameState = -1;

	private Vector3 CapsuleBottomSphereCenter => physicsCollider.transform.TransformPoint(checkSphereVerticalOffset);

	private bool IsGrounded() =>
		Physics.CheckSphere(CapsuleBottomSphereCenter + isGroundedCheckOffset, checkSphereRadius, groundLayer);

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();

		checkSphereVerticalOffset =
			(physicsCollider.center.y - physicsCollider.height / 2.0f + physicsCollider.radius) * Vector3.up;
		checkSphereRadius = transform.localScale.y * physicsCollider.radius * GroundCheckSphereScale;
	}

	private void ResetVelocityIfMatchEnded(int newGameState)
	{
		if (previousGameState == newGameState)
			return;

		previousGameState = newGameState;

		if ((GameState)newGameState == GameState.MatchEnded)
		{
			rigidbody.velocity = Vector3.zero;
		}
	}

	public void ProcessMovement(float forwardMovementValue,
		float rightMovementValue,
		bool jump)
	{
		if (deathController.IsDead.Value)
			return;

		Vector3 inputVector = new Vector3(forwardMovementValue, 0, rightMovementValue);
		Vector3 movementDirection = inputVector != Vector3.zero
			? this.transform.TransformDirection(inputVector.normalized)
			: Vector3.zero;

		bool isGrounded = ManageGroundedState();

		ApplyMovement(movementDirection, isGrounded);
		ProcessJumping(jump, isGrounded);
	}

	private bool ManageGroundedState()
	{
		bool isGrounded = IsGrounded();
		rigidbody.useGravity = !isGrounded; // to prevent sliding from slopes

		IsGroundedStateUpdate.Invoke(isGrounded);
		return isGrounded;
	}

	private void ApplyMovement(Vector3 movementDirection,
		bool isGrounded)
	{
		Vector3 targetVelocity = movementDirection * movementSpeed;
		Vector3 effectiveVelocity =
			Vector3.MoveTowards(rigidbody.velocity, targetVelocity, Elympics.TickDuration * acceleration);
		effectiveVelocity.y = rigidbody.velocity.y;

		rigidbody.velocity = isGrounded ? GetSlopeAdjustedVelocity(effectiveVelocity) : effectiveVelocity;

		MovementValuesChanged?.Invoke(movementDirection);
	}

	// to prevent bouncing when going down the slope and jumping when stopping on the slope when going up
	private Vector3 GetSlopeAdjustedVelocity(Vector3 currentVelocity)
	{
		var groundContact = GetGroundContactProperties();

		if (groundContact == null)
			return NeutralizeVerticalBoost(currentVelocity);

		// match velocity direction with the ground / slope
		Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, groundContact.Norm);
		Vector3 adjustedVelocity = slopeRotation * currentVelocity;

		float slopeAngleCos = Mathf.Cos(groundContact.SlopeAngle);
		Vector3 groundForward =
			Vector3.Cross(Vector3.Cross(groundContact.Norm, Vector3.up), groundContact.Norm).normalized;

		// fix velocity vector to match desired horizontal speed
		if (adjustedVelocity.y <= 0)
		{
			Vector3 velocityForwardComponent = Vector3.Dot(groundForward, adjustedVelocity) * groundForward;

			return adjustedVelocity + -velocityForwardComponent + velocityForwardComponent / slopeAngleCos;
		}
		else
		{
			Vector3 velocityForwardComponent = Vector3.Dot(groundForward, currentVelocity) * groundForward;

			return ZeroY(currentVelocity + -velocityForwardComponent +
			             velocityForwardComponent / Mathf.Pow(slopeAngleCos, 2));
		}
	}

	private GroundContact GetGroundContactProperties()
	{
		Vector3 currentCapsuleBottomSphereCenterPosition = CapsuleBottomSphereCenter; // cached for minor optimization
		float minAngle = MaxClimbAngle;
		Vector3 chosenNorm = Vector3.zero;

		// get possible ground colliders
		var touchingColliders = Physics.OverlapSphere(currentCapsuleBottomSphereCenterPosition + isGroundedCheckOffset,
			checkSphereRadius, groundLayer);

		if (touchingColliders.Length == 0)
			return null;

		foreach (var collider in touchingColliders)
		{
			Vector3 norm = currentCapsuleBottomSphereCenterPosition -
			               collider.ClosestPoint(currentCapsuleBottomSphereCenterPosition);
			float slopeAngle = Vector3.Angle(Vector3.up, norm) * Mathf.Deg2Rad;

			if (slopeAngle < minAngle)
			{
				chosenNorm = norm;
				minAngle = slopeAngle;
			}
		}

		if (minAngle == MaxClimbAngle)
			return null;

		return new GroundContact(minAngle, chosenNorm);
	}

	private Vector3 NeutralizeVerticalBoost(Vector3 currentVelocity)
	{
		return currentVelocity.y <= 0 ? currentVelocity : ZeroY(currentVelocity);
	}

	private Vector3 ZeroY(Vector3 vector)
	{
		return new Vector3(vector.x, 0, vector.z);
	}

	private void ProcessJumping(bool jump,
		bool isGrounded)
	{
		if (jump)
		{
			if (isGrounded && !jumpedInPreviousFrame)
				ApplyJump();
		}
		else
		{
			jumpedInPreviousFrame = false;
		}
	}

	private void ApplyJump()
	{
		rigidbody.velocity = ZeroY(rigidbody.velocity);
		rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

		jumpedInPreviousFrame = true;

		PlayerJumped?.Invoke();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(CapsuleBottomSphereCenter + isGroundedCheckOffset, checkSphereRadius);
	}

	public void ElympicsUpdate()
	{
		ResetVelocityIfMatchEnded(gameStateController.CurrentGameState.Value);
	}
}