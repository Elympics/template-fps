using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerThirdPersonAnimatorMovementController : MonoBehaviour
{
	[SerializeField] private MovementController playerMovementController;
	[SerializeField] private DeathController playerDeathController;

	private readonly int movementForwardParameterHash = Animator.StringToHash("MovementForward");
	private readonly int movementRightParameterHash = Animator.StringToHash("MovementRight");
	private readonly int jumpingTriggerParameterHash = Animator.StringToHash("JumpTrigger");
	private readonly int deathTriggerParameterHash = Animator.StringToHash("DeathTrigger");
	private readonly int resetTriggerParameterHash = Animator.StringToHash("ResetTrigger");
	private readonly int isGroundedParameterHash = Animator.StringToHash("IsGrounded");

	private Animator thirdPersonAnimator = null;

	private void Awake()
	{
		thirdPersonAnimator = GetComponent<Animator>();
		playerMovementController.MovementValuesChanged += ProcessMovementValues;
		playerMovementController.PlayerJumped += ProcessJumping;
		playerMovementController.IsGroundedStateUpdate += ProcessIsGroundedStateUpdate;
		playerDeathController.IsDead.ValueChanged += ProcessDeathState;
	}

	private void ProcessDeathState(bool lastValue, bool newValue)
	{
		if (newValue)
		{
			thirdPersonAnimator.SetTrigger(deathTriggerParameterHash);
		}
		else
		{
			thirdPersonAnimator.SetTrigger(resetTriggerParameterHash);
		}
	}

	private void ProcessIsGroundedStateUpdate(bool isGrounded)
	{
		thirdPersonAnimator.SetBool(isGroundedParameterHash, isGrounded);
	}

	private void ProcessJumping()
	{
		thirdPersonAnimator.SetTrigger(jumpingTriggerParameterHash);
	}

	private void ProcessMovementValues(Vector3 movementDirection)
	{
		var localMovementDirection = playerMovementController.transform.InverseTransformDirection(movementDirection) * 2.0f;

		thirdPersonAnimator.SetFloat(movementForwardParameterHash, localMovementDirection.z);
		thirdPersonAnimator.SetFloat(movementRightParameterHash, localMovementDirection.x);
	}
}
